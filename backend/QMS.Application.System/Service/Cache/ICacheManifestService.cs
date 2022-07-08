namespace QMS.Application.System.Service.Cache
{
    public interface ICacheManifestService
    {
        Task<Dictionary<string, string>> GetAllCache();

        Task<IEnumerable<string>> GetAllCacheKeys();

        Task RemoveAllCache();

        Task RemoveAllPermisson();

        Task RemoveAllMenu();

        Task RemoveUserIdCache(long id);

        Task RemoveUserCache(string input);
    }
}