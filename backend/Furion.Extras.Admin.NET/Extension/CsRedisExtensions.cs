using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Furion.Extras.Admin.NET.Extension;

/// <summary>
/// csredis扩展
/// </summary>
public static class CsRedisExtensions
{
    /// <summary>
    /// 使用缓存
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static void UseCsRedis(this IServiceCollection services)
    {
        var redisStr = $"{App.Configuration["Cache:RedisConnectionString"]},prefix={App.Configuration["Cache:InstanceName"]}";

        //csredis的两种使用方式
        var csredis = new CSRedisClient(redisStr);
        services.AddSingleton(csredis);
        RedisHelper.Initialization(csredis);

        //基于redis初始化IDistributedCache
        services.AddSingleton<IDistributedCache>(new CSRedisCache(csredis));
    }
}
