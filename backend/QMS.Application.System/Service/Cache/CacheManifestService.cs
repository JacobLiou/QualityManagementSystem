using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.System.Service.Cache
{
    /// <summary>
    /// 页面缓存清单服务
    /// </summary>
    [ApiDescriptionSettings(Name = "cachemanifest", Order = 170)]
    public class CacheManifestService : IDynamicApiController, ITransient, ICacheManifestService
    {
        private readonly ICacheService _cache;

        public CacheManifestService(ICacheService cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 获取全部缓存键
        /// </summary>
        /// <returns></returns>
        [HttpGet("system/cachemanifest/getallcachekeys")]
        public async Task<IEnumerable<string>> GetAllCacheKeys()
        {
            return await _cache.GetCache<IEnumerable<string>>(CacheKeys.CACHE_ALL_KEY);
        }

        /// <summary>
        /// 获取全部缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet("system/cachemanifest/getallcache")]
        public async Task<Dictionary<string, string>> GetAllCache()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            IEnumerable<string> list = await GetAllCacheKeys();
            if (list != null && list.Count() > 0)
            {
                foreach (string key in list)
                {
                    var value = await _cache.GetCache<string>(key);
                    dict[key] = value;
                }
            }
            return dict;
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet("system/cachemanifest/removeallcache")]
        public async Task RemoveAllCache()
        {
            List<string> list = new List<string>();
            list.Add(CacheKeys.CACHE_ALL_KEY);
            await _cache.RemoveCache(list);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [HttpPost("system/cachemanifest/removecache")]
        public async Task RemoveCache(IEnumerable<string> keys)
        {
            await _cache.RemoveCache(keys);
        }
    }
}