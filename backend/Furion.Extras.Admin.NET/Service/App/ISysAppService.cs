using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysAppService
    {
        Task AddApp(AddAppInput input);

        Task DeleteApp(BaseId input);

        Task<SysApp> GetApp([FromQuery] QueryAppInput input);

        Task<List<SysApp>> GetAppList();

        Task<List<AppOutput>> GetLoginApps(long userId);

        Task<PageResult<SysApp>> QueryAppPageList([FromQuery] AppPageInput input);

        Task SetAsDefault(SetDefaultAppInput input);

        Task UpdateApp(UpdateAppInput input);

        Task ChangeUserAppStatus(ChangeUserAppStatusInput input);
    }
}