using QMS.Core;

namespace QMS.Application.System.Service.Version
{
    public interface IVersionServices
    {
        Task AddVersion(string type, string version);

        Task DeleteVersion(string type, string version);

        Task<List<SysVersion>> GetAllTypeList();

        Task UpdateVersion(long id, string type, string version);

        Task<List<SysVersion>> GetTypeVersionList(string type);
    }
}