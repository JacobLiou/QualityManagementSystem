using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Furion.Extras.Admin.NET.Service
{
    /// <summary>
    /// 在线用户服务
    /// </summary>
    [ApiDescriptionSettings(Name = "OnlineUser", Order = 100)]
    public class SysOnlineUserService : ISysOnlineUserService, IDynamicApiController, ITransient
    {
        private readonly ISysCacheService _sysCacheService;
        private readonly IRepository<SysUser> _sysUserRep;// 用户表仓储
        private readonly IRepository<SysTenant, MultiTenantDbContextLocator> _sysTenantRep;// 租户仓储
        private readonly IHubContext<ChatHub, IChatClient> _chatHubContext;

        public SysOnlineUserService(ISysCacheService sysCacheService, IRepository<SysUser> sysUerRep,
            IRepository<SysTenant, MultiTenantDbContextLocator> sysTenantRep,
            IHubContext<ChatHub, IChatClient> chatHubContext)
        {
            _sysCacheService = sysCacheService;
            _sysUserRep = sysUerRep;
            _sysTenantRep = sysTenantRep;
            _chatHubContext = chatHubContext;
        }

        /// <summary>
        /// 分页获取在线用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysOnlineUser/page")]
        public async Task<PageResult<OnlineUserOutput>> QueryOnlineUserPageList([FromQuery] PageInputBase input)
        {
            var onlineUsers = await _sysCacheService.GetAsync<List<OnlineUser>>(CommonConst.CACHE_KEY_ONLINE_USER) ?? new List<OnlineUser>();
            var currUser = _sysUserRep.FirstOrDefault(u => u.Id == CurrentUserInfo.UserId, false);
            var onlineUserOutputs = onlineUsers
                .Where(!CurrentUserInfo.IsSuperAdmin, o => o.TenantId == currUser.TenantId)
                .Where(!string.IsNullOrWhiteSpace(input.SearchValue), o => o.Account.Contains(input.SearchValue) || o.Name.Contains(input.SearchValue))
                .Select(o => o.Adapt<OnlineUserOutput>());

            var currentPageData = onlineUserOutputs.ToADPagedList(input.PageNo, input.PageSize);

            //填充租户名称
            var tenants = await _sysTenantRep.DetachedEntities.ToListAsync();
            currentPageData.Rows.ToList().ForEach(o => o.TenantName = tenants.FirstOrDefault(p => p.Id == o.TenantId)?.Name);

            return currentPageData;
        }

        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysOnlineUser/list")]
        public async Task<List<OnlineUserOutput>> List()
        {
            var onlineUsers = await _sysCacheService.GetAsync<List<OnlineUser>>(CommonConst.CACHE_KEY_ONLINE_USER) ?? new List<OnlineUser>();
            var currUser = _sysUserRep.FirstOrDefault(u => u.Id == CurrentUserInfo.UserId, false);
            var onlineUserOutputs = onlineUsers
                .Where(!CurrentUserInfo.IsSuperAdmin, o => o.TenantId == currUser.TenantId)
                .Select(o => o.Adapt<OnlineUserOutput>())
                .ToList();

            //填充租户名称
            var tenants = await _sysTenantRep.DetachedEntities.ToListAsync();
            onlineUserOutputs.ForEach(o => o.TenantName = tenants.FirstOrDefault(p => p.Id == o.TenantId)?.Name);

            return onlineUserOutputs;
        }

        /// <summary>
        /// 强制下线
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("/sysOnlineUser/forceExist")]
        public async Task ForceExist([FromBody] OnlineUser user)
        {
            await _chatHubContext.Clients.Client(user.ConnectionId).ForceExist();
        }
    }
}