using Furion.DependencyInjection;
using Furion.Extras.Admin.NET;
using Furion.JsonSerialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core
{
    public class QMSDistributedCache: ITransient
    {
        IDistributedCache _cache;
        public IServiceProvider Services { get; }
        public QMSDistributedCache(IServiceProvider services, IDistributedCache distributedCache)
        {
            _cache = distributedCache;
            Services = services;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetAsync(string cacheKey, object value)
        {
            await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JSON.Serialize(value)));

            await AddCacheKey(cacheKey);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetStringAsync(string cacheKey, string value, int minutes)
        {
            await _cache.SetStringAsync(cacheKey, value, this.GetCacheEntryOptions(minutes));

            await AddCacheKey(cacheKey);
        }

        public async Task SetStringAsync(string cacheKey, string value, DistributedCacheEntryOptions cacheOption)
        {
            await _cache.SetStringAsync(cacheKey, value, cacheOption);

            await AddCacheKey(cacheKey);
        }

        private DistributedCacheEntryOptions GetCacheEntryOptions(int minutes)
        {
            DistributedCacheEntryOptions cacheOption = new DistributedCacheEntryOptions();
            cacheOption.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));

            return cacheOption;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetStringAsync(string cacheKey, string value)
        {
            await _cache.SetStringAsync(cacheKey, value);

            await AddCacheKey(cacheKey);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string cacheKey)
        {
            return await _cache.GetStringAsync(cacheKey);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string cacheKey)
        {
            var res = await _cache.GetAsync(cacheKey);
            return res == null ? default : JSON.Deserialize<T>(Encoding.UTF8.GetString(res));
        }
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);

            await DelCacheKey(key);
        }

        public async Task RefreshAsync(string key)
        {
            await _cache.RefreshAsync(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public bool Exists(string cacheKey)
        {
            return _cache.Equals(cacheKey);
        }

        /// <summary>
        /// 增加缓存Key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private async Task AddCacheKey(string cacheKey)
        {
            var res = await _cache.GetStringAsync(CommonConst.CACHE_KEY_ALL);
            var allkeys = string.IsNullOrWhiteSpace(res) ? new HashSet<string>() : JSON.Deserialize<HashSet<string>>(res);
            if (!allkeys.Any(m => m == cacheKey))
            {
                allkeys.Add(cacheKey);
                await _cache.SetStringAsync(CommonConst.CACHE_KEY_ALL, JSON.Serialize(allkeys));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private async Task DelCacheKey(string cacheKey)
        {
            var res = await _cache.GetStringAsync(CommonConst.CACHE_KEY_ALL);
            var allkeys = string.IsNullOrWhiteSpace(res) ? new HashSet<string>() : JSON.Deserialize<HashSet<string>>(res);
            if (allkeys.Any(m => m == cacheKey))
            {
                allkeys.Remove(cacheKey);
                await _cache.SetStringAsync(CommonConst.CACHE_KEY_ALL, JSON.Serialize(allkeys));
            }
        }


    }
}
