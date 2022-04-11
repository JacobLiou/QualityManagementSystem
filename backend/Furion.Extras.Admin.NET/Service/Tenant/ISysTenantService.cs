using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysTenantService
    {
        Task AddTenant(AddTenantInput input);

        Task DeleteTenant(DeleteTenantInput input);

        Task<SysTenant> GetTenant([FromQuery] QueryTenantInput input);

        Task GrantMenu(GrantRoleMenuInput input);

        Task InitNewTenant(SysTenant newTenant);

        Task<List<long>> OwnMenu([FromQuery] QueryTenantInput input);

        Task<PageResult<TenantOutput>> QueryTenantPageList([FromQuery] TenantPageInput input);

        Task ResetUserPwd(QueryTenantInput input);

        Task UpdateTenant(UpdateTenantInput input);
    }
}