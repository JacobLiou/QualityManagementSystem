using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using Yitter.IdGenerator;

namespace Furion.Extras.Admin.NET.Service
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [ApiDescriptionSettings(Name = "User", Order = 150)]
    public class SysUserService : ISysUserService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysUser> _sysUserRep;  // 用户表仓储
        private readonly ISysCacheService _sysCacheService;
        private readonly ISysEmpService _sysEmpService;
        private readonly ISysUserDataScopeService _sysUserDataScopeService;
        private readonly ISysUserRoleService _sysUserRoleService;
        private readonly ISysOrgService _sysOrgService;
        private readonly IRepository<SysOauthUser> _sysOauthUserRep;  // 用户表仓储

        public SysUserService(IRepository<SysUser> sysUserRep,
                              ISysCacheService sysCacheService,
                              ISysEmpService sysEmpService,
                              ISysUserDataScopeService sysUserDataScopeService,
                              ISysUserRoleService sysUserRoleService,
                              ISysOrgService sysOrgService,
                              IRepository<SysOauthUser> sysOauthUserRep)
        {
            _sysUserRep = sysUserRep;
            _sysCacheService = sysCacheService;
            _sysEmpService = sysEmpService;
            _sysUserDataScopeService = sysUserDataScopeService;
            _sysUserRoleService = sysUserRoleService;
            _sysOrgService = sysOrgService;
            _sysOauthUserRep = sysOauthUserRep;
        }

        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/page")]
        public async Task<PageResult<UserOutput>> QueryUserPageList([FromQuery] UserPageInput input)
        {
            var searchValue = input.SearchValue;
            var pid = input.SysEmpParam.OrgId;

            var sysEmpRep = Db.GetRepository<SysEmp>();
            var sysOrgRep = Db.GetRepository<SysOrg>();
            var dataScopes = await GetUserDataScopeIdList(CurrentUserInfo.UserId);
            var users = await _sysUserRep.DetachedEntities
                                         .Join(sysEmpRep.DetachedEntities, u => u.Id, e => e.Id, (u, e) => new { u, e })
                                         .Join(sysOrgRep.DetachedEntities, n => n.e.OrgId, o => o.Id, (n, o) => new { n, o })
                                         .Where(!string.IsNullOrEmpty(searchValue), x => (x.n.u.Account.Contains(input.SearchValue) ||
                                                                                    x.n.u.Name.Contains(input.SearchValue) ||
                                                                                    x.n.u.Phone.Contains(input.SearchValue)))
                                         .Where(!string.IsNullOrEmpty(pid), x => (x.n.e.OrgId == long.Parse(pid) ||
                                                                            x.o.Pids.Contains($"[{pid.Trim()}]")))
                                         .Where(x => input.SearchStatus.Contains(x.n.u.Status))
                                         .Where(x => x.n.u.AdminType != AdminType.SuperAdmin)//排除超级管理员
                                         .Where(!CurrentUserInfo.IsSuperAdmin && dataScopes.Count > 0, x => dataScopes.Contains(x.n.e.OrgId))
                                         .Select(u => u.n.u.Adapt<UserOutput>())
                                         .ToADPagedListAsync(input.PageNo, input.PageSize);

            foreach (var user in users.Rows)
            {
                user.SysEmpInfo = await _sysEmpService.GetEmpInfo(long.Parse(user.Id));
            }
            return users;
        }

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/add")]
        public async Task AddUser(AddUserInput input)
        {
            // 数据范围检查
            CheckDataScope(input.SysEmpParam.OrgId);

            var isExist = await _sysUserRep.AnyAsync(u => u.Account == input.Account && !u.IsDeleted, false, true);
            if (isExist) throw Oops.Oh(ErrorCode.D1003);

            var user = input.Adapt<SysUser>();
            user.Password = MD5Encryption.Encrypt(input.Password);
            if (string.IsNullOrEmpty(user.Name))
                user.Name = user.Account;
            if (string.IsNullOrEmpty(user.NickName))
                user.NickName = user.Account;
            var newUser = await _sysUserRep.InsertNowAsync(user);
            input.SysEmpParam.Id = newUser.Entity.Id.ToString();
            // 增加员工信息
            await _sysEmpService.AddOrUpdate(input.SysEmpParam);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/delete")]
        public async Task DeleteUser(DeleteUserInput input)
        {
            // 数据范围检查
            CheckDataScope(input.SysEmpParam.OrgId);

            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id, false);
            if (user == null)
                throw Oops.Oh(ErrorCode.D1002);

            if (user.AdminType == AdminType.SuperAdmin)
                throw Oops.Oh(ErrorCode.D1014);

            if (user.AdminType == AdminType.Admin)
                throw Oops.Oh(ErrorCode.D1018);

            if (user.Id == CurrentUserInfo.UserId)
                throw Oops.Oh(ErrorCode.D1001);

            // 直接删除用户
            await user.DeleteAsync();

            // 删除员工及附属机构职位信息
            await _sysEmpService.DeleteEmpInfoByUserId(input.Id);//empId与userId相同

            //删除该用户对应的用户-角色表关联信息
            await _sysUserRoleService.DeleteUserRoleListByUserId(input.Id);

            //删除该用户对应的用户-数据范围表关联信息
            await _sysUserDataScopeService.DeleteUserDataScopeListByUserId(input.Id);

            //如果企业微信记录表存在对应的用户记录则删除
            var oauthUser = await _sysOauthUserRep.FirstOrDefaultAsync(u => u.OpenId == input.Id.ToString(), false);
            if (oauthUser != null)
            {
                await oauthUser.DeleteAsync();
            }
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/edit")]
        public async Task UpdateUser(UpdateUserInput input)
        {
            // 数据范围检查
            CheckDataScope(input.SysEmpParam.OrgId);

            // 排除自己并且判断与其他是否相同
            var isExist = await _sysUserRep.AnyAsync(u => u.Account == input.Account && u.Id != input.Id, false);
            if (isExist) throw Oops.Oh(ErrorCode.D1003);

            var user = input.Adapt<SysUser>();
            await user.UpdateExcludeAsync(new[] { nameof(SysUser.Password), nameof(SysUser.Status), nameof(SysUser.AdminType) }, true);
            input.SysEmpParam.Id = user.Id.ToString();
            // 更新员工及附属机构职位信息
            await _sysEmpService.AddOrUpdate(input.SysEmpParam);
        }

        /// <summary>
        /// 查看用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysUser/detail")]
        public async Task<UserOutput> GetUser(long id)
        {
            var user = await _sysUserRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == id);
            var userDto = user.Adapt<UserOutput>();
            if (userDto != null)
            {
                userDto.SysEmpInfo = await _sysEmpService.GetEmpInfo(user.Id);
            }
            return userDto;
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/changeStatus")]
        public async Task ChangeUserStatus(UpdateUserStatusInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (user.AdminType == AdminType.SuperAdmin)
                throw Oops.Oh(ErrorCode.D1015);

            if (!Enum.IsDefined(typeof(CommonStatus), input.Status))
                throw Oops.Oh(ErrorCode.D3005);
            user.Status = input.Status;
        }

        /// <summary>
        /// 授权用户角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/grantRole")]
        public async Task GrantUserRole(UpdateUserRoleDataInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (user.AdminType == AdminType.SuperAdmin)
                throw Oops.Oh(ErrorCode.D1022);

            if (user.AdminType == AdminType.Admin)
                throw Oops.Oh(ErrorCode.D1008);

            // 数据范围检查
            CheckDataScope(input.SysEmpParam.OrgId);
            await _sysUserRoleService.GrantRole(input);
        }

        /// <summary>
        /// 授权用户数据范围
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/grantData")]
        public async Task GrantUserData(UpdateUserRoleDataInput input)
        {
            // 清除缓存
            await _sysCacheService.RemoveAsync(CommonConst.CACHE_KEY_DATASCOPE + $"{input.Id}");

            // 数据范围检查
            CheckDataScope(input.SysEmpParam.OrgId);
            await _sysUserDataScopeService.GrantData(input);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/updateInfo")]
        public async Task UpdateUserInfo(UpdateUserBaseInfoInput input)
        {
            var user = input.Adapt<SysUser>();
            await user.UpdateExcludeAsync(new[] { nameof(SysUser.AdminType), nameof(SysUser.LastLoginIp), nameof(SysUser.LastLoginTime) });
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/updatePwd")]
        public async Task UpdateUserPwd(ChangePasswordUserInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (MD5Encryption.Encrypt(input.Password) != user.Password)
                throw Oops.Oh(ErrorCode.D1004);
            user.Password = MD5Encryption.Encrypt(input.NewPassword);
        }

        /// <summary>
        /// 获取用户拥有角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/ownRole")]
        public async Task<List<long>> GetUserOwnRole([FromQuery] QueryUserInput input)
        {
            return await _sysUserRoleService.GetUserRoleIdList(input.Id);
        }

        /// <summary>
        /// 获取用户拥有数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysUser/ownData")]
        public async Task<List<long>> GetUserOwnData([FromQuery] QueryUserInput input)
        {
            return await _sysUserDataScopeService.GetUserDataScopeIdList(input.Id);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/resetPwd")]
        public async Task ResetUserPwd(QueryUserInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            user.Password = MD5Encryption.Encrypt(CommonConst.DEFAULT_PASSWORD);
        }

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/updateAvatar")]
        public async Task UpdateAvatar(UploadAvatarInput input)
        {
            var user = await _sysUserRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            user.Avatar = input.Avatar.ToString();
        }

        /// <summary>
        /// 获取用户选择器
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous] //公告中需要使用，开放权限
        [HttpGet("/sysUser/selector")]
        public async Task<List<UserOutput>> GetUserSelector([FromQuery] UserSelectorInput input)
        {
            var name = !string.IsNullOrEmpty(input.Name?.Trim());
            var result = await _sysUserRep.DetachedEntities
                                    .Where(name, u => EF.Functions.Like(u.Name, $"%{input.Name.Trim()}%"))
                                    .Where(u => u.Status != CommonStatus.DELETED)
                                    .Where(u => u.AdminType != AdminType.SuperAdmin)
                                    .ToListAsync();
            return result.Adapt<List<UserOutput>>();
        }

        /// <summary>
        /// 用户导出
        /// </summary>
        /// <returns></returns>
        [HttpGet("/sysUser/export")]
        public async Task<IActionResult> ExportUser()
        {
            var users = _sysUserRep.DetachedEntities.AsQueryable();

            var list = users.ToList();

            var memoryStream = new MemoryStream();
            memoryStream.SaveAs(list);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return await Task.FromResult(new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "user.xlsx"
            });
        }

        /// <summary>
        /// 用户导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/sysUser/import")]
        public async Task ImportUser(IFormFile file)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{YitIdHelper.NextId()}.xlsx");
            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            //var rows = MiniExcel.Query(path); // 解析
            //foreach (var row in rows)
            //{
            //    var a = row.A;
            //    var b = row.B;
            //    // 入库等操作

            //}
        }

        /// <summary>
        /// 根据用户Id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<SysUser> GetUserById(long userId)
        {
            return await _sysUserRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// 将OAuth账号转换成账号
        /// </summary>
        /// <param name="authUser"></param>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        [NonAction]
        public async Task SaveAuthUserToUser(AuthUserInput authUser, CreateUserInput sysUser)
        {
            var user = sysUser.Adapt<SysUser>();
            user.AdminType = AdminType.None; // 非管理员

            // oauth账号与系统账号判断
            var isExist = await _sysUserRep.DetachedEntities.AnyAsync(u => u.Account == authUser.Username);
            user.Account = isExist ? authUser.Username + DateTime.Now.Ticks : authUser.Username;
            user.Name = user.NickName = authUser.Nickname;
            user.Email = authUser.Email;
            user.Sex = authUser.Gender;
            await user.InsertAsync();
        }

        /// <summary>
        /// 获取用户数据范围（机构Id集合）并缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList(long userId)
        {
            var dataScopes = await _sysCacheService.GetDataScope(userId); // 先从缓存里面读取
            if (dataScopes == null || dataScopes.Count < 1)
            {
                if (!CurrentUserInfo.IsSuperAdmin)
                {
                    var orgId = await _sysEmpService.GetEmpOrgId(userId);
                    // 获取该用户对应的数据范围集合
                    var userDataScopeIdListForUser = await _sysUserDataScopeService.GetUserDataScopeIdList(userId);
                    // 获取该用户的角色对应的数据范围集合
                    var userDataScopeIdListForRole = await _sysUserRoleService.GetUserRoleDataScopeIdList(userId, orgId);
                    dataScopes = userDataScopeIdListForUser.Concat(userDataScopeIdListForRole).Distinct().ToList(); // 并集
                }
                else
                {
                    dataScopes = await _sysOrgService.GetAllDataScopeIdList();
                }
                await _sysCacheService.SetDataScope(userId, dataScopes); // 缓存结果
            }
            return dataScopes;
        }

        /// <summary>
        /// 获取用户数据范围（机构Id集合）
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList()
        {
            var userId = CurrentUserInfo.UserId;
            var dataScopes = await GetUserDataScopeIdList(userId);
            return dataScopes;
        }

        /// <summary>
        /// 检查普通用户数据范围
        /// 当有用户有多个组织时，在登录时选择一个组织，所以组织id（orgId）从前端传过来
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        private async void CheckDataScope(string orgId)
        {
            // 如果当前用户不是超级管理员，则进行数据范围校验
            if (!CurrentUserInfo.IsSuperAdmin)
            {
                var dataScopes = await GetUserDataScopeIdList(CurrentUserInfo.UserId);
                if (dataScopes == null || (orgId != null && !dataScopes.Any(u => u == long.Parse(orgId))))
                    throw Oops.Oh(ErrorCode.D1013);
            }
        }

        [HttpGet("/sysUser/tree")]
        public async Task<dynamic> GetOrgUserTree()
        {
            var dataScopeList = new List<long>();
            if (!CurrentUserInfo.IsSuperAdmin)
            {
                var dataScopes = await this.GetUserDataScopeIdList();
                if (dataScopes.Count < 1)
                    return dataScopeList;
                dataScopeList = GetDataScopeList(dataScopes);
            }

            IRepository<SysOrg> repository = App.GetService<IRepository<SysOrg>>();

            var orgs = await repository.DetachedEntities.Where(dataScopeList.Count > 0, u => dataScopeList.Contains(u.Id))
                                                        .Where(u => u.Status == CommonStatus.ENABLE)
                                                        .OrderBy(u => u.Sort)
                                                        .ProjectToType<OrgTreeNode>()
                                                        .ToListAsync();

            return new TreeBuildUtil<OrgTreeNode>().Build(orgs);
        }

        private List<long> GetDataScopeList(List<long> dataScopes)
        {
            var dataScopeList = new List<long>();
            // 如果是超级管理员则获取所有组织机构，否则只获取其数据范围的机构数据
            if (!CurrentUserInfo.IsSuperAdmin)
            {
                if (dataScopes.Count < 1)
                    return dataScopeList;

                IRepository<SysOrg> repository = App.GetService<IRepository<SysOrg>>();

                // 此处获取所有的上级节点，用于构造完整树
                dataScopes.ForEach(u =>
                {
                    var sysOrg = repository.DetachedEntities.FirstOrDefault(c => c.Id == u);
                    if (sysOrg != null)
                    {
                        var parentAndChildIdListWithSelf = sysOrg.Pids.TrimEnd(',').Replace("[", "").Replace("]", "")
                                                                    .Split(",").Select(u => long.Parse(u)).ToList();
                        parentAndChildIdListWithSelf.Add(sysOrg.Id);
                        dataScopeList.AddRange(parentAndChildIdListWithSelf);
                    }
                });
            }

            return dataScopeList;
        }
    }
}