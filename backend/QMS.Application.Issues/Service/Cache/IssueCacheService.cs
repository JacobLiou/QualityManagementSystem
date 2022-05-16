using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using QMS.Application.Issues.Helper;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题扩展属性服务
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "IssueColumn", Order = 100)]
    public class IssueCacheService : IDynamicApiController, ISingleton
    {
        private readonly IDistributedCache _cache;

        public IssueCacheService(
            IDistributedCache cache
        )
        {
            this._cache = cache;
        }

        [NonAction]
        public async Task<string> GetUserColumns(long userId)
        {
            var cacheKey = Constants.USER_COLUMNS + userId;
            var res = await _cache.GetStringAsync(cacheKey);
            return res;
        }

        [NonAction]
        public async Task SetUserColumns(long userId, string json)
        {
            var cacheKey = Constants.USER_COLUMNS + userId;

            DistributedCacheEntryOptions cacheOption = new DistributedCacheEntryOptions();
            cacheOption.SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(cacheKey, json, cacheOption);
        }
    }
}
