using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.JsonSerialization;
using Microsoft.Extensions.Caching.Distributed;
using QMS.Core;
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
        private readonly QMSDistributedCache _cache;

        public CacheService(QMSDistributedCache cache)
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
        public async Task SetCache<T>(string cacheKey, T value, int hours, int minutes, int seconds)
        {
            string json = "";
            TimeSpan time = new TimeSpan(hours, minutes, seconds);
            DistributedCacheEntryOptions option = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = time };
            if (typeof(T) == typeof(string))
            {
                json = value.ToString();
            }
            else
            {
                json = JSON.Serialize(value);
            }
            await _cache.SetStringAsync(cacheKey, json, option);
            //await _cache.SetObjectAsync(cacheKey, value, option);
        }

        /// <summary>
        ///设置缓存时间（按小时）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public async Task SetCacheByHours<T>(string cacheKey, T value, int hours)
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
        public async Task SetCacheByMinutes<T>(string cacheKey, T value, int minutes)
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
        public async Task SetCacheBySecond<T>(string cacheKey, T value, int seconds)
        {
            await SetCache(cacheKey, value, 0, 0, seconds);
        }

        /// <summary>
        /// 设置缓存（不设置过期时间）
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetCache<T>(string cacheKey, T value)
        {
            string json = "";
            if (typeof(T) == typeof(string))
            {
                json = value.ToString();
            }
            else
            {
                json = JSON.Serialize(value);
            }
            await _cache.SetStringAsync(cacheKey, json);
            //await _cache.SetObjectAsync(cacheKey, value);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<T> GetCache<T>(string cacheKey)
        {
            var value = await _cache.GetStringAsync(cacheKey);
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            if (!string.IsNullOrEmpty(value))
            {
                return JSON.Deserialize<T>(value);
            }
            return default(T);
            //return await _cache.GetObjectAsync<T>(cacheKey);
        }

        /// <summary>
        /// 批量删除缓存键值
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task RemoveCache(IEnumerable<string> keys)
        {
            foreach (string key in keys.Distinct())
            {
                _cache.RemoveAsync(key);
            }
        }

        /// <summary>
        /// 更新缓存键值
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task RefreshCache(IEnumerable<string> keys)
        {
            foreach (string key in keys.Distinct())
            {
                _cache.RefreshAsync(key);
            }
        }
    }
}