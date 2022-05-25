using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using QMS.Application.Issues.Helper;
using QMS.Core;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题信息缓存服务
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

            await _cache.SetStringAsync(cacheKey, json, this.GetCacheEntryOptions());
        }

        [NonAction]
        public async Task<string> GetFieldsStruct()
        {
            var cacheKey = Constants.FIELD_STRUCT;

            var res = await _cache.GetStringAsync(cacheKey);
            return res;
        }

        [NonAction]
        public async Task SetFieldsStruct(string fieldStructDicStr)
        {
            var cacheKey = Constants.FIELD_STRUCT;

            await _cache.SetStringAsync(cacheKey, fieldStructDicStr, this.GetCacheEntryOptions(120));
        }

        [NonAction]
        public async Task<string> GetUserName(long userId)
        {
            var cacheKey = CoreCommonConst.USERID + userId;

            var res = await _cache.GetStringAsync(cacheKey);
            return res;
        }

        [NonAction]
        public async Task SetUserName(long userId, string objStr)
        {
            var cacheKey = CoreCommonConst.USERID + userId;

            await _cache.SetStringAsync(cacheKey, objStr, this.GetCacheEntryOptions());
        }


        [NonAction]
        public async Task<string> GetProjectName(long projectId)
        {
            var cacheKey = CoreCommonConst.PROJECTID + projectId;

            var res = await _cache.GetStringAsync(cacheKey);
            return res;
        }

        [NonAction]
        public async Task<string> GetProductName(long productId)
        {
            var cacheKey = CoreCommonConst.PRODUCTID + productId;

            var res = await _cache.GetStringAsync(cacheKey);
            return res;
        }

        private DistributedCacheEntryOptions GetCacheEntryOptions(int minutes = 30)
        {
            DistributedCacheEntryOptions cacheOption = new DistributedCacheEntryOptions();
            cacheOption.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));

            return cacheOption;
        }
    }
}
