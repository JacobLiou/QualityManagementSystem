namespace QMS.Application.System
{
    public interface ICacheService
    {
        Task<string> GetStringCache(string cacheKey);

        Task SetStringCache(string cacheKey, string value, int hours, int minutes, int seconds);

        Task SetStringCacheByHours(string cacheKey, string value, int hours);

        Task SetStringCacheByMinutes(string cacheKey, string value, int minutes);

        Task SetStringCacheBySecond(string cacheKey, string value, int seconds);
    }
}