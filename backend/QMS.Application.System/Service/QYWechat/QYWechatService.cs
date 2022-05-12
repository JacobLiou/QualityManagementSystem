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
        private readonly QYWeChatOAuth _qyWechatOAuth;
        private readonly IRepository<SysUser> _sysUserRep; // 用户表仓储
        private readonly ISysEmpService _sysEmpService; // 系统员工服务

        public QYWechatService(IHttpContextAccessor httpContextAccessor, QYWeChatOAuth qyWechatOAuth, IRepository<SysUser> repository, ISysEmpService sysEmpService)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _qyWechatOAuth = qyWechatOAuth;
            _sysUserRep = repository;
            _sysEmpService = sysEmpService;
        }

        /// <summary>
        /// 企业微信登录授权
        /// </summary>
        [HttpGet("system/qyWechat/login")]
        public Task QYWechatLogin()
        {
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
        [HttpPost("system/qyWechat/loginAndRegister")]
        public string QYWechatLoginRegister(string code)
        {
            //获取企业微信扫码用户详细信息
            QYTokenModel token = _qyWechatOAuth.GetAccessTokenAsync().Result;
            QYUserIdModel userId = _qyWechatOAuth.GetQYUserIdAsync(token.AccessToken, code).Result;
            QYUserInfoModel userInfo = _qyWechatOAuth.GetQYUserInfoAsync(token.AccessToken, userId.UserId).Result;

            //判断用户是否已经存在
            var user = _sysUserRep.Where(u => u.Account.Equals(userInfo.Account) || u.Phone.Equals(userInfo.Phone) || u.Email.Equals(userInfo.Email)).FirstOrDefault();

            if (user == null)
            {
                //用户不存在则登录
                user = _qyWechatOAuth.QYWechatRegister(userInfo).Result;
            }
            //调用登录接口
            var accessToken = _qyWechatOAuth.QYWechatLogin(user);
            return accessToken;
        }
    }
}