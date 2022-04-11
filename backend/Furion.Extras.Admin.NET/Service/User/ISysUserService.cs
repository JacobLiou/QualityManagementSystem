using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysUserService
    {
        Task AddUser(AddUserInput input);

        Task ChangeUserStatus(UpdateUserStatusInput input);

        Task DeleteUser(DeleteUserInput input);

        Task<IActionResult> ExportUser();

        Task<UserOutput> GetUser(long id);

        Task<SysUser> GetUserById(long userId);

        Task<List<long>> GetUserDataScopeIdList();

        Task<List<long>> GetUserDataScopeIdList(long userId);

        Task<List<long>> GetUserOwnData([FromQuery] QueryUserInput input);

        Task<List<long>> GetUserOwnRole([FromQuery] QueryUserInput input);

        Task<List<UserOutput>> GetUserSelector([FromQuery] UserSelectorInput input);

        Task GrantUserData(UpdateUserRoleDataInput input);

        Task GrantUserRole(UpdateUserRoleDataInput input);

        Task ImportUser(IFormFile file);

        Task<PageResult<UserOutput>> QueryUserPageList([FromQuery] UserPageInput input);

        Task ResetUserPwd(QueryUserInput input);

        Task SaveAuthUserToUser(AuthUserInput authUser, CreateUserInput sysUser);

        Task UpdateAvatar(UploadAvatarInput input);

        Task UpdateUser(UpdateUserInput input);

        Task UpdateUserInfo(UpdateUserBaseInfoInput input);

        Task UpdateUserPwd(ChangePasswordUserInput input);
    }
}