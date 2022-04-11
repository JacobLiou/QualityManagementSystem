using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysConfigService
    {
        Task AddConfig(AddConfigInput input);

        Task DeleteConfig(DeleteConfigInput input);

        Task<SysConfig> GetConfig([FromQuery] QueryConfigInput input);

        Task<List<SysConfig>> GetConfigList();

        Task<PageResult<SysConfig>> QueryConfigPageList([FromQuery] ConfigPageInput input);

        Task UpdateConfig(UpdateConfigInput input);

        Task<bool> GetDemoEnvFlag();

        Task<bool> GetCaptchaOpenFlag();

        Task UpdateConfigCache(string code, string value);
    }
}