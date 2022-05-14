using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.JsonSerialization;
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
    public class CacheService<T> : IDynamicApiController, ICacheService<T>, ITransient
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 设置缓存，泛型方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public async Task SetCache(string cacheKey, T value, int hours, int minutes, int seconds)
        {
            TimeSpan time = new TimeSpan(hours, minutes, seconds);
            DistributedCacheEntryOptions option = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = time };
            var json = JSON.Serialize(value);
            await _cache.SetStringAsync(cacheKey, json, option);
        }

        /// <summary>
        ///设置缓存时间（按小时）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public async Task SetCacheByHours(string cacheKey, T value, int hours)
        {
            await SetCache(cacheKey, value, hours, 0, 0);
        }

        /// <summary>
        /// 设置缓存时间（按分钟）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public async Task SetCacheByMinutes(string cacheKey, T value, int minutes)
        {
            await SetCache(cacheKey, value, 0, minutes, 0);
        }

        /// <summary>
        ///设置缓存时间（按秒）
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public async Task SetCacheBySecond(string cacheKey, T value, int seconds)
        {
            await SetCache(cacheKey, value, 0, 0, seconds);
        }

        /// <summary>
        /// 获取缓存-引用类型
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<T> GetCache(string cacheKey)
        {
            var value = await _cache.GetStringAsync(cacheKey);
            if (value == null)
            {
                value = "";
            }
            return JSON.Deserialize<T>(value);
        }
    }
}