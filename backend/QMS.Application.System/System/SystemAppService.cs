using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using QMS.Core;

namespace QMS.Application.System
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    [ApiDescriptionSettings("质量管理基础数据", Name = "System", Order = 100)]
    public class SystemAppService : IDynamicApiController
    {
        private readonly ISystemService _systemService;
        private readonly IDistributedCache _cache;
        public SystemAppService(ISystemService systemService, IDistributedCache cache)
        {
            _systemService = systemService;
            _cache = cache;
        }


        /// <summary>
        ///  获取人员和组
        /// </summary>
        /// <returns></returns>
        [HttpGet("/System/UserGroup")]
        public async Task<List<GroupUserOutput>> GetUserGroup()
        {
            var userGruop = _cache.GetObject<List<GroupUserOutput>>(CacheKeys.CachedUserGroup);
            if (userGruop == null)
            {
                userGruop = _systemService.GetUserGroup();

                DistributedCacheEntryOptions cacheOption = new DistributedCacheEntryOptions();
                cacheOption.SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); //设置10分钟后过期
                _cache.SetObject(CacheKeys.CachedUserGroup, userGruop, cacheOption);
            }
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
        [HttpGet("/System/group")]
        public List<SsuGroupOutput> GetGroup()
        {
            return _systemService.GetGroup();
        }
    }
}