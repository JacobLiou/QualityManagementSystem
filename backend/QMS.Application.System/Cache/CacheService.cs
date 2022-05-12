using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    /// <summary>
    /// 自定义缓存服务类
    /// </summary>
    public class CacheService : IDynamicApiController, ICacheService, ITransient
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public async Task SetStringCache(string cacheKey, string value, int hours, int minutes, int seconds)
        {
            TimeSpan time = new TimeSpan(hours, minutes, seconds);
            DistributedCacheEntryOptions option = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = time };
            await _cache.SetStringAsync(cacheKey, value, option);
        }

        /// <summary>
        ///设置缓存时间（按小时）
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public async Task SetStringCacheByHours(string cacheKey, string value, int hours)
        {
            await SetStringCache(cacheKey, value, hours, 0, 0);
        }

        /// <summary>
        ///设置缓存时间（按分钟）
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public async Task SetStringCacheByMinutes(string cacheKey, string value, int minutes)
        {
            await SetStringCache(cacheKey, value, 0, minutes, 0);
        }

        /// <summary>
        ///设置缓存时间（按秒）
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public async Task SetStringCacheBySecond(string cacheKey, string value, int seconds)
        {
            await SetStringCache(cacheKey, value, 0, 0, seconds);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<string> GetStringCache(string cacheKey)
        {
            return await _cache.GetStringAsync(cacheKey);
        }
    }
}