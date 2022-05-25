using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly string LoginUrl = "http://qms.sofarsolar.com:8002/user/login";

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
        /// 获取企业微信扫码登录URL
        /// </summary>
        /// <returns></returns>
        [HttpGet("system/qyWechat/qywechatloginurl")]
        public string QYWechatLoginUrl()
        {
            return _qyWechatOAuth.GetAuthorizeUrl();
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
        public Task QYWechatLoginRegister([FromQuery] string code)
        {
            var param = new Dictionary<string, string>()
            {
                ["access_token"] = this.QYWechatGetLoginToken(code)
            };
            _httpContext.Response.Redirect($"{LoginUrl}?{param.ToQueryString()}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 企业微信登录用户后返回jwt token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string QYWechatGetLoginToken(string code)
        {
            //获取企业微信扫码用户详细信息
            QYTokenModel token = _qyWechatOAuth.GetAccessTokenAsync().Result;
            QYUserIdModel userId = _qyWechatOAuth.GetQYUserIdAsync(token.AccessToken, code).Result;
            QYUserInfoModel userInfo = _qyWechatOAuth.GetQYUserInfoAsync(token.AccessToken, userId.UserId).Result;

            //判断用户是否已经存在,取消租户查询（IgnoreQueryFilters）
            var user = _sysUserRep.DetachedEntities.IgnoreQueryFilters().Where(u => u.Account.Equals(userInfo.Account) || u.Phone.Equals(userInfo.Phone) || u.Email.Equals(userInfo.Email)).FirstOrDefault();

            //用户不存在则注册
            user = _qyWechatOAuth.QYWechatRegister(userInfo, user);

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
        [HttpPost("system/qyWechat/sendMessageQYWechatID")]
        public string QYWechatSendMessage(IEnumerable<string> touser, string toparty, string totag, string title, string description, string url)
        {
            return _qyWechatOAuth.QYWechatSendMessage(touser, toparty, totag, title, description, url).Result;
        }


        /// <summary>
        /// 企业微信发送文本卡片消息
        /// </summary>
        /// <param name="touser">接收消息用户UserId列表</param>
        /// <param name="toparty">接收消息部门</param>
        /// <param name="totag">消息标签</param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost("system/qyWechat/sendMessageUserId")]
        public string QYWechatSendMessage(string touser, string toparty, string totag, string title, string description, string url)
        {
            return _qyWechatOAuth.QYWechatSendMessage(touser, toparty, totag, title, description, url).Result;
        }
    }
}