using Furion;
using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Options;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;

namespace QMS.Application.System
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginVerify : ILoginVerify, ITransient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISysEmpService _sysEmpService; // 系统员工服务

        public LoginVerify(IHttpContextAccessor httpContextAccessor, IEventPublisher eventPublisher, ISysEmpService sysEmpService)
        {
            _httpContextAccessor = httpContextAccessor;
            _eventPublisher = eventPublisher;
            _sysEmpService = sysEmpService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string Login(SysUser user)
        {
            // 员工信息
            var empInfo = _sysEmpService.GetEmpInfo(user.Id).Result;

            // 生成Token令牌
            //var accessToken = await _jwtBearerManager.CreateTokenAdmin(user);
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                {ClaimConst.CLAINM_USERID, user.Id},
                {ClaimConst.TENANT_ID, user.TenantId},
                {ClaimConst.CLAINM_ACCOUNT, user.Account},
                {ClaimConst.CLAINM_NAME, user.Name},
                {ClaimConst.CLAINM_SUPERADMIN, user.AdminType},
                {ClaimConst.CLAINM_ORGID, empInfo.OrgId},
                {ClaimConst.CLAINM_ORGNAME, empInfo.OrgName},
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