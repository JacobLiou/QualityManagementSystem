using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using QMS.Application.System.Service.Log;
using QMS.Core;
using QMS.Core.Entity;

namespace QMS.Application.System
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    [ApiDescriptionSettings("问题管理Demo", Name = "group", Order = 100)]
    [Route("system/[controller]")]
    public class GroupAppService : IDynamicApiController
    {
        private readonly ISystemService _systemService;
        private readonly IDistributedCache _cache;
        private readonly ILogDebugService _logDebugService;
        public GroupAppService(ISystemService systemService, IDistributedCache cache, ILogDebugService logDebugService)
        {
            _systemService = systemService;
            _cache = cache;
            _logDebugService = logDebugService;
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input">
        /// groupId：用户组ID 
        /// </param>
        /// <remarks>备注说明<br />
        /// 返回用户组<br />
        /// GroupName：组名<br />
        /// GroupID：组ID<br />
        /// UserName：用户<br />
        /// UserID：用户ID
        /// </remarks>
        /// <returns></returns>
        [HttpGet("usergroup")]
        public async Task<List<GroupUserOutput>> GetUserGroup(long groupId = 281695421571141)
        {
            var userGruop = _cache.GetObject<List<GroupUserOutput>>(CacheKeys.CachedUserGroup + groupId);
            if (userGruop == null)
            {
                userGruop = _systemService.GetUserGroup(groupId);

                

                DistributedCacheEntryOptions cacheOption = new DistributedCacheEntryOptions();
                cacheOption.SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); //设置10分钟后过期
                _cache.SetObject(CacheKeys.CachedUserGroup + groupId, userGruop, cacheOption);
            }

            SysLogDebug logDebug = new SysLogDebug();
            logDebug.ClassName = this.GetType().Name;
            logDebug.Method = "GetUserGroup";
            logDebug.DebugName = "测试日志";
            logDebug.DebugContext  = _cache.ToString();
            logDebug.CreatedTime = DateTime.Now;

            _logDebugService.AddLogDebug(logDebug);


            return userGruop;
        }




        /// <summary>
        /// 获取系统描述
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return _systemService.GetDescription();
        }


        /// <summary>
        /// 获取组
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public List<SsuGroupOutput> GetGroup()
        {
            return _systemService.GetGroup();
        }
    }
}