namespace QMS.Application.System.Service.Cache
{
    public interface ICacheManifestService
    {
        Task<Dictionary<string, string>> GetAllCache();
        Task<IEnumerable<string>> GetAllCacheKeys();
        Task RemoveAllCache();
        Task RemoveCache(IEnumerable<string> keys);
    }
}