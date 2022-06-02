namespace QMS.Application.System
{
    public interface ICacheService
    {
        Task<T> GetCache<T>(string cacheKey);

        Task SetCache<T>(string cacheKey, T value, int hours, int minutes, int seconds);

        Task SetCacheByHours<T>(string cacheKey, T value, int hours);

        Task SetCacheByMinutes<T>(string cacheKey, T value, int minutes);

        Task SetCacheBySecond<T>(string cacheKey, T value, int seconds);

        Task SetCache<T>(string cacheKey, T value);

        Task RemoveCache(IEnumerable<string> keys);

        Task RefreshCache(IEnumerable<string> keys);
    }
}