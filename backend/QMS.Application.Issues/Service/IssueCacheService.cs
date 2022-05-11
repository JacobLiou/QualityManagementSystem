using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.JsonSerialization;
using Microsoft.Extensions.Caching.Distributed;
using QMS.Application.Issues.Helper;
using QMS.Core;
using QMS.Core.Entity;

namespace QMS.Application.Issues.Service
{
    internal class IssueCacheService : ISingleton
    {
        private readonly IDistributedCache _cache;
        //private readonly IRepository<SsuIssueColumnDisplay, IssuesDbContextLocator> _ssuIssueColumnDisplayRep;


        public IssueCacheService(
            IDistributedCache cache
            //IRepository<SsuIssueColumnDisplay, IssuesDbContextLocator> ssuIssueColumnDisplayRep
        )
        {
            this._cache = cache;
            //this._ssuIssueColumnDisplayRep = ssuIssueColumnDisplayRep;
        }

        public async Task<KeyValuePair<string, string>[]> GetUserColumns(long userId)
        {
            var cacheKey = Constants.USER_COLUMNS + userId;
            var res = await _cache.GetStringAsync(cacheKey);
            return string.IsNullOrWhiteSpace(res) ? Constants.USER_COLUMN_NAMES : JSON.Deserialize<KeyValuePair<string, string>[]>(res);
        }

        public async Task SetUserColumns(long userId, string json)
        {
            var cacheKey = Constants.USER_COLUMNS + userId;
            await _cache.SetStringAsync(cacheKey, json);
        }
    }
}
