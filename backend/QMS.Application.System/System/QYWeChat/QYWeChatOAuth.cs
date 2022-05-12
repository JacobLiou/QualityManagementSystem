using Furion;
using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Options;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Furion.RemoteRequest.Extensions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace QMS.Application.System
{
    /// <summary>
    /// 企业微信扫码登录
    /// </summary>
    public class QYWeChatOAuth : IQYWeChatOAuth, ITransient
    {
        private readonly ICacheService _cache;
        private readonly IRepository<SysUser> _sysUserRep;  // 用户表仓储
        private readonly IRepository<SysOauthUser> _sysOauthUserRep;  // oauth用户表仓储
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventPublisher _eventPublisher;

        //企业微信重定向至登录界面
        private readonly string RedirectUrl = "https://open.work.weixin.qq.com/wwopen/sso/qrConnect";

        //企业微信获取access_token信息
        private readonly string AccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";

        //获取扫码用户ID
        private readonly string UserIdUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo";

        //获取扫码用户信息
        private readonly string UserInfoUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/get";

        private readonly string Appid = "ww44dc5ededfe4a954";
        private readonly string Corpsecret = "4-WoTdHkSNnUbnpxhI3PT1pAZTRmerr9AtsMV-HweDY";
        private readonly string Agentid = "1000017";
        private readonly string Status = "web_login@gyoss9";
        private readonly string LoginUrl = "http%3A%2F%2Fqms.sofarsolar.com:8001";
        private readonly int CacheHour = 2;

        public QYWeChatOAuth(ICacheService cache, IRepository<SysUser> user, IRepository<SysOauthUser> oauthUser, IHttpContextAccessor httpContextAccessor
            , IEventPublisher eventPublisher)
        {
            _cache = cache;
            _sysUserRep = user;
            _sysOauthUserRep = oauthUser;
            _httpContextAccessor = httpContextAccessor;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// 发起授权
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizeUrl()
        {
            var param = new Dictionary<string, string>()
            {
                ["appid"] = Appid,
                ["agentid"] = Agentid,
                ["redirect_uri"] = LoginUrl,
                ["state"] = Status
            };
            return $"{RedirectUrl}?{param.ToQueryString()}#wechat_redirect";
        }

        /// <summary>
        /// 获取企业微信Token
        /// </summary>
        /// <returns></returns>
        public async Task<QYTokenModel> GetAccessTokenAsync()
        {
            QYTokenModel accessTokenModel = new QYTokenModel();
            if (!string.IsNullOrEmpty(_cache.GetStringCache(Appid).Result))
            {
                accessTokenModel.ErrMsg = "ok";
                accessTokenModel.ErrorCode = 0;
                accessTokenModel.AccessToken = _cache.GetStringCache(Appid).Result;
                return accessTokenModel;
            }
            var param = new Dictionary<string, string>()
            {
                ["corpid"] = Appid,
                ["corpsecret"] = Corpsecret
            };
            accessTokenModel = await $"{AccessTokenUrl}?{param.ToQueryString()}".GetAsAsync<QYTokenModel>();
            if (accessTokenModel.HasError())
                throw Oops.Oh($"{ accessTokenModel.ErrMsg}");
            await _cache.SetStringCacheByHours(Appid, accessTokenModel.AccessToken, CacheHour);
            return accessTokenModel;
        }

        /// <summary>
        /// 获取微信用户ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<QYUserIdModel> GetQYUserIdAsync(string accessToken, string code)
        {
            var param = new Dictionary<string, string>()
            {
                ["access_token"] = accessToken,
                ["code"] = code
            };
            var userIdModel = await $"{UserIdUrl}?{param.ToQueryString()}".GetAsAsync<QYUserIdModel>();
            if (userIdModel.HasError())
                throw Oops.Oh($"{ userIdModel.ErrMsg}");
            return userIdModel;
        }

        /// <summary>
        /// 获取企业微信用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<QYUserInfoModel> GetQYUserInfoAsync(string accessToken, string userId)
        {
            var paramUserInfo = new Dictionary<string, string>()
            {
                ["access_token"] = accessToken,
                ["userid"] = userId
            };
            var userInfoModel = await $"{UserInfoUrl}?{paramUserInfo.ToQueryString()}".GetAsAsync<QYUserInfoModel>();
            if (userInfoModel.HasError())
                throw Oops.Oh($"{userInfoModel.ErrMsg}");
            return userInfoModel;
        }

        /// <summary>
        /// 企业微信注册
        /// </summary>
        /// <param name="qYUserInfo"></param>
        /// <returns></returns>
        public async Task<SysUser> QYWechatRegister(QYUserInfoModel qYUserInfo)
        {
            var user = qYUserInfo.Adapt<SysUser>();
            user.Password = MD5Encryption.Encrypt("123456");
            var newUser = await _sysUserRep.InsertNowAsync(user);

            var oauthUser = qYUserInfo.Adapt<SysOauthUser>();
            oauthUser.OpenId = newUser.Entity.Id.ToString();
            await _sysOauthUserRep.InsertNowAsync(oauthUser);
            return newUser.Entity;
        }

        /// <summary>
        /// 企业微信登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string QYWechatLogin(SysUser user)
        {
            // 员工信息
            //var empInfo = _sysEmpService.GetEmpInfo(user.Id).Result;

            // 生成Token令牌
            //var accessToken = await _jwtBearerManager.CreateTokenAdmin(user);
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                {ClaimConst.CLAINM_USERID, user.Id},
                {ClaimConst.TENANT_ID, user.TenantId},
                {ClaimConst.CLAINM_ACCOUNT, user.Account},
                {ClaimConst.CLAINM_NAME, user.Name},
                {ClaimConst.CLAINM_SUPERADMIN, user.AdminType},
                //{ClaimConst.CLAINM_ORGID, empInfo.OrgId},
                //{ClaimConst.CLAINM_ORGNAME, empInfo.OrgName},
            });

            // 设置Swagger自动登录
            _httpContextAccessor.HttpContext.SigninToSwagger(accessToken);

            // 生成刷新Token令牌
            var refreshToken =
                JWTEncryption.GenerateRefreshToken(accessToken, App.GetOptions<RefreshTokenSettingOptions>().ExpiredTime);

            // 设置刷新Token令牌
            _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            // 增加登录日志
            _eventPublisher.PublishAsync(new ChannelEventSource("Create:VisLog",
                new SysLogVis
                {
                    Name = user.Name,
                    Success = YesOrNot.Y,
                    Message = "登录成功",
                    Ip = user.LastLoginIp,
                    VisType = LoginType.LOGIN,
                    VisTime = user.LastLoginTime,
                    Account = user.Account
                }));

            return accessToken;
        }
    }
}