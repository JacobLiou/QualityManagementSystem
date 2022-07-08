using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
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
                foreach (string key in list.Distinct())
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
            IEnumerable<string> list = await GetAllCacheKeys();
            await _cache.RemoveCache(list.Distinct());
        }

        /// <summary>
        /// 清除所有权限缓存(permission_)
        /// </summary>
        /// <returns></returns>
        [HttpPost("system/cachemanifest/removeallpermisson")]
        public async Task RemoveAllPermisson()
        {
            IEnumerable<string> list = await GetAllCacheKeys();
            var newList = list.Where(u => u.Contains("permission_"));
            if (newList != null && newList.Count() > 0)
            {
                await _cache.RemoveCache(newList.Distinct());
            }
        }

        /// <summary>
        /// 清除所有菜单缓存
        /// </summary>
        /// <returns></returns>
        [HttpPost("system/cachemanifest/removeallmenu")]
        public async Task RemoveAllMenu()
        {
            IEnumerable<string> list = await GetAllCacheKeys();
            var newList = list.Where(u => u.Contains("menu_"));
            if (newList != null && newList.Count() > 0)
            {
                await _cache.RemoveCache(newList.Distinct());
            }
        }

        /// <summary>
        /// 清除指定用户ID的缓存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("system/cachemanifest/removeuseridcache")]
        public async Task RemoveUserIdCache(long id)
        {
            IEnumerable<string> list = await GetAllCacheKeys();
            var newList = list.Where(u => u.Contains(id.ToString()));
            if (newList != null && newList.Count() > 0)
            {
                await _cache.RemoveCache(newList.Distinct());
            }
        }

        /// <summary>
        /// 清除指定用户名或者指定账户名的缓存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("system/cachemanifest/removeusernamecache")]
        public async Task RemoveUserCache(string input)
        {
            var userService = App.GetService<IRepository<SysUser>>();
            var user = userService.DetachedEntities.FirstOrDefault(u => u.Name == input || u.Account == input);
            if (user != null)
            {
                await this.RemoveUserIdCache(user.Id);
            }
        }
    }
}