using Furion;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
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
        private readonly QMSDistributedCache _cache;

        public IssueCacheService(
            QMSDistributedCache cache
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

            await _cache.SetStringAsync(cacheKey, json, 30);
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

            await _cache.SetStringAsync(cacheKey, fieldStructDicStr, 120);
        }

        /// <summary>
        /// 获取租户Id
        /// </summary>
        /// <returns></returns>
        public string GetTenantId()
        {
            if (App.User == null) return string.Empty;
            //这个Convert，嗯，有用
            return App.User.FindFirst(ClaimConst.TENANT_ID)?.Value + "_";
        }

        [NonAction]
        public async Task<string> GetUserName(long userId)
        {
            var cacheKey = CoreCommonConst.USERID + this.GetTenantId() + userId;

            var res = await _cache.GetStringAsync(cacheKey);
            return res;
        }


        [NonAction]
        public async Task SetUserName(long userId, string objStr)
        {
            var cacheKey = CoreCommonConst.USERID + this.GetTenantId() + userId;

            await _cache.SetStringAsync(cacheKey, objStr, 30);
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

        public async Task<string> GetUserByProjectModularId(long projectId, long modularId)
        {
            var cacheKey = CoreCommonConst.PROJECT_MODULAR + projectId + "_" + modularId;

            var res = await _cache.GetStringAsync(cacheKey);
            return res;
        }

        

        [NonAction]
        public async Task SetString(string key, string value)
        {
            await _cache.SetStringAsync(key, value);
        }
        [NonAction]
        public bool Exists(string cacheKey)
        {
            return _cache.Equals(cacheKey);
        }
        [NonAction]
        public async Task<string> GetString(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        public async Task SetString(string key, string value, int hours, int minutes, int seconds)
        {
            DistributedCacheEntryOptions cacheOption = new DistributedCacheEntryOptions();
            TimeSpan time = new TimeSpan(hours, minutes, seconds);
            DistributedCacheEntryOptions option = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = time };
            await _cache.SetStringAsync(key, value, cacheOption);
        }
    }
}