﻿using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.EventBus;
using Furion.Extras.Admin.NET.Options;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UAParser;

namespace Furion.Extras.Admin.NET.Service
{
    /// <summary>
    /// 登录授权相关服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Auth", Order = 160)]
    public class AuthService : IAuthService, IDynamicApiController, ITransient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IRepository<SysUser> _sysUserRep; // 用户表仓储
        private readonly ISysUserService _sysUserService; // 系统用户服务
        private readonly ISysEmpService _sysEmpService; // 系统员工服务
        private readonly ISysRoleService _sysRoleService; // 系统角色服务
        private readonly ISysMenuService _sysMenuService; // 系统菜单服务
        private readonly ISysAppService _sysAppService; // 系统应用服务
        private readonly IClickWordCaptcha _captchaHandle; // 验证码服务
        private readonly ISysConfigService _sysConfigService; // 验证码服务
        private readonly IEventPublisher _eventPublisher;
        private readonly ISysCacheService _cache;
        public AuthService(IRepository<SysUser> sysUserRep, IHttpContextAccessor httpContextAccessor,
            ISysUserService sysUserService, ISysEmpService sysEmpService, ISysRoleService sysRoleService,
            ISysMenuService sysMenuService, ISysAppService sysAppService, IClickWordCaptcha captchaHandle,
            ISysConfigService sysConfigService, IEventPublisher eventPublisher, ISysCacheService cache)
        {
            _sysUserRep = sysUserRep;
            _httpContextAccessor = httpContextAccessor;
            _sysUserService = sysUserService;
            _sysEmpService = sysEmpService;
            _sysRoleService = sysRoleService;
            _sysMenuService = sysMenuService;
            _sysAppService = sysAppService;
            _captchaHandle = captchaHandle;
            _sysConfigService = sysConfigService;
            _eventPublisher = eventPublisher;
            _cache = cache;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input">
        /// Account：用户名 <br />
        /// Password：密码
        /// </param>
        /// <remarks>用户登录接口备注说明<br />
        /// 返回授权的accessToken </remarks>
        /// <returns></returns>
        [HttpPost("/login")]
        [AllowAnonymous]
        public string LoginAsync([Required] LoginInput input)
        {
            // 获取加密后的密码
            var encryptPasswod = MD5Encryption.Encrypt(input.Password);

            // 判断用户名和密码是否正确 忽略全局过滤器
            var user = _sysUserRep
                .Where(u => u.Account.Equals(input.Account) && u.Password.Equals(encryptPasswod) && !u.IsDeleted, false, true)
                .FirstOrDefault();
            _ = user ?? throw Oops.Oh(ErrorCode.D1000);

            // 验证账号是否被冻结
            if (user.Status == CommonStatus.DISABLE)
                throw Oops.Oh(ErrorCode.D1017);

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

            return accessToken;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/emailLogin")]
        [AllowAnonymous]
        public string EmailLoginAsync([Required] EmailLoginInput input)
        {

            // 判断用户名和密码是否正确 忽略全局过滤器
            var user = _sysUserRep
                .Where(u => u.Email.Equals(input.Email) && !u.IsDeleted, false, true)
                .FirstOrDefault();
            _ = user ?? throw Oops.Oh(ErrorCode.D1024);

            var cacheCaptcha = _cache.GetStringAsync(CommonConst.CACHE_PHONE_CODE + "_" + input.Email).Result;
            if (!input.Captcha.Equals(cacheCaptcha))
                throw Oops.Oh(ErrorCode.D1025);

            // 验证账号是否被冻结
            if (user.Status == CommonStatus.DISABLE)
                throw Oops.Oh(ErrorCode.D1017);

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

            return accessToken;
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/getLoginUser")]
        public async Task<LoginOutput> GetLoginUserAsync()
        {
            var user = _sysUserRep.FirstOrDefault(u => u.Id == CurrentUserInfo.UserId, false);
            if (user == null)
                throw Oops.Oh(ErrorCode.D1011);
            var userId = user.Id;

            var httpContext = _httpContextAccessor.HttpContext;
            var loginOutput = user.Adapt<LoginOutput>();

            loginOutput.LastLoginTime = user.LastLoginTime = DateTimeOffset.Now;
            loginOutput.LastLoginIp = user.LastLoginIp = httpContext.GetRequestIPv4();

            //var ipInfo = IpTool.Search(loginOutput.LastLoginIp);
            //loginOutput.LastLoginAddress = ipInfo.Country + ipInfo.Province + ipInfo.City + "[" + ipInfo.NetworkOperator + "][" + ipInfo.Latitude + ipInfo.Longitude + "]";

            var client = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
            loginOutput.LastLoginBrowser = client.UA.Family + client.UA.Major;
            loginOutput.LastLoginOs = client.OS.Family + client.OS.Major;

            // 员工信息
            loginOutput.LoginEmpInfo = await _sysEmpService.GetEmpInfo(userId);

            // 角色信息
            loginOutput.Roles = await _sysRoleService.GetUserRoleList(userId);

            // 权限信息
            loginOutput.Permissions = await _sysMenuService.GetLoginPermissionList(userId);

            // 系统所有权限信息
            loginOutput.AllPermissions = await _sysMenuService.GetAllPermissionList();

            // 数据范围信息(机构Id集合)
            loginOutput.DataScopes = await _sysUserService.GetUserDataScopeIdList(userId);

            // 具备应用信息（多系统，默认激活一个，可根据系统切换菜单）,返回的结果中第一个为激活的系统
            loginOutput.Apps = await _sysAppService.GetLoginApps(userId);

            // 菜单信息
            if (loginOutput.Apps.Count > 0)
            {
                var activeApp = loginOutput.Apps.FirstOrDefault(u => u.Active == YesOrNot.Y.ToString());
                var defaultActiveAppCode = activeApp != null ? activeApp.Code : loginOutput.Apps.FirstOrDefault().Code;
                loginOutput.Menus = await _sysMenuService.GetLoginMenusAntDesign(userId, defaultActiveAppCode);
            }

            // 更新用户最后登录Ip和时间
            await _sysUserRep.UpdateIncludeAsync(user, new[] { nameof(SysUser.LastLoginIp), nameof(SysUser.LastLoginTime) });

            // 增加登录日志
            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:VisLog",
                new SysLogVis
                {
                    Name = loginOutput.Name,
                    Success = YesOrNot.Y,
                    Message = "登录成功",
                    Ip = loginOutput.LastLoginIp,
                    Browser = loginOutput.LastLoginBrowser,
                    Os = loginOutput.LastLoginOs,
                    VisType = LoginType.LOGIN,
                    VisTime = loginOutput.LastLoginTime,
                    Account = loginOutput.Account
                }));
            return loginOutput;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpGet("/logout")]
        [AllowAnonymous]
        public async Task LogoutAsync()
        {
            var ip = _httpContextAccessor.HttpContext.GetRequestIPv4();
            _httpContextAccessor.HttpContext.SignoutToSwagger();
            //_httpContextAccessor.HttpContext.Response.Headers["access-token"] = "invalid token";

            // 增加退出日志
            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:VisLog",
                new SysLogVis
                {
                    Name = CurrentUserInfo.Name,
                    Success = YesOrNot.Y,
                    Message = "退出成功",
                    VisType = LoginType.LOGOUT,
                    VisTime = DateTimeOffset.Now,
                    Account = CurrentUserInfo.Account,
                    Ip = ip
                }));
        }

        /// <summary>
        /// 获取验证码开关
        /// </summary>
        /// <returns></returns>
        [HttpGet("/getCaptchaOpen")]
        [AllowAnonymous]
        public async Task<bool> GetCaptchaOpen()
        {
            return await _sysConfigService.GetCaptchaOpenFlag();
        }

        /// <summary>
        /// 获取验证码（默认点选模式）
        /// </summary>
        /// <returns></returns>
        [HttpPost("/captcha/get")]
        [AllowAnonymous]
        [NonUnify]
        public async Task<ClickWordCaptchaResult> GetCaptcha()
        {
            // 图片大小要与前端保持一致（坐标范围）
            return await _captchaHandle.CreateCaptchaImage(_captchaHandle.RandomCode(4), 310, 155);
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/captcha/check")]
        [AllowAnonymous]
        [NonUnify]
        public async Task<ClickWordCaptchaResult> VerificationCode(ClickWordCaptchaInput input)
        {
            return await _captchaHandle.CheckCode(input);
        }
    }
}