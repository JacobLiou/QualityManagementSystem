namespace QMS.Application.System
{
    public interface ICacheService<T>
    {
        Task<T> GetCache(string cacheKey);

        Task SetCache(string cacheKey, T value, int hours, int minutes, int seconds);

        Task SetCacheByHours(string cacheKey, T value, int hours);

        Task SetCacheByMinutes(string cacheKey, T value, int minutes);

        Task SetCacheBySecond(string cacheKey, T value, int seconds);
    }
}