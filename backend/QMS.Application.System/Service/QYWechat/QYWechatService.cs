using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.System
{
    /// <summary>
    /// 企业微信接口服务类
    /// </summary>
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "qyWechat", Order = 160)]
    public class QYWechatService : IDynamicApiController, ITransient, IQYWechatService
    {
        private readonly HttpContext _httpContext;
        private readonly IQYWeChatOAuth _qyWechatOAuth;
        private readonly IRepository<SysUser> _sysUserRep; // 用户表仓储
        private readonly IRepository<SysOauthUser> _sysOauthUserRep; // 用户表仓储
        private readonly ISysEmpService _sysEmpService; // 系统员工服务

        public QYWechatService(IHttpContextAccessor httpContextAccessor, QYWeChatOAuth qyWechatOAuth, IRepository<SysUser> sysUser, IRepository<SysOauthUser> sysOauth, ISysEmpService sysEmpService)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _qyWechatOAuth = qyWechatOAuth;
            _sysUserRep = sysUser;
            _sysOauthUserRep = sysOauth;
            _sysEmpService = sysEmpService;
        }

        /// <summary>
        /// 发起授权
        /// </summary>
        [HttpGet("system/qyWechat/login")]
        public Task QYWechatLogin()
        {
            //return _qyWechatOAuth.GetAuthorizeUrl();
            _httpContext.Response.Redirect(_qyWechatOAuth.GetAuthorizeUrl());
            return Task.CompletedTask;
        }

        /// <summary>
        /// 企业微信登录授权回调
        /// </summary>
        /// <param name="error_description"></param>
        /// <returns></returns>
        [HttpGet("system/qyWechat/callBack")]
        public async Task QYWechatLoginCallback([FromQuery] string error_description = "")
        {
            if (!string.IsNullOrEmpty(error_description))
                throw Oops.Oh(error_description);

            var accessTokenModel = await _qyWechatOAuth.GetAccessTokenAsync();
            await _httpContext.Response.WriteAsJsonAsync(accessTokenModel);
        }

        /// <summary>
        /// 获取微信用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("system/qyWechat/getUserInfo")]
        public async Task<dynamic> GetWechatUserInfo([FromQuery] string token, [FromQuery] string userId)
        {
            return await _qyWechatOAuth.GetQYUserInfoAsync(token, userId);
        }


        /// <summary>
        /// 企业微信根据返回值登录
        /// </summary>
        /// <param name="code">企业微信登录扫码后从URL中获取的code值</param>
        /// <returns></returns>
        [HttpGet("system/qyWechat/loginAndRegister")]
        public string QYWechatLoginRegister([FromQuery] string code)
        {
            //获取企业微信扫码用户详细信息
            QYTokenModel token = _qyWechatOAuth.GetAccessTokenAsync().Result;
            QYUserIdModel userId = _qyWechatOAuth.GetQYUserIdAsync(token.AccessToken, code).Result;
            QYUserInfoModel userInfo = _qyWechatOAuth.GetQYUserInfoAsync(token.AccessToken, userId.UserId).Result;

            //判断用户是否已经存在
            var user = _sysUserRep.Where(u => u.Account.Equals(userInfo.Account) || u.Phone.Equals(userInfo.Phone) || u.Email.Equals(userInfo.Email)).FirstOrDefault();

            //用户不存在则注册
            user = _qyWechatOAuth.QYWechatRegister(userInfo, user).Result;

            //调用登录接口
            var accessToken = _qyWechatOAuth.QYWechatLogin(user);
            return accessToken;
        }

        /// <summary>
        /// 企业微信发送文本卡片消息
        /// </summary>
        /// <param name="touser">接收消息用户列表</param>
        /// <param name="toparty">接收消息部门</param>
        /// <param name="totag">消息标签</param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost("system/qyWechat/sendMessage")]
        public string QYWechatSendMessage(string[] touser, string toparty, string totag, string title, string description, string url)
        {
            return _qyWechatOAuth.QYWechatSendMessage(touser, toparty, totag, title, description, url).Result;
        }
    }
}