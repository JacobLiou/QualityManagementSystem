using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Furion.FriendlyException;
using Furion.DatabaseAccessor.Extensions;
using System.Linq.Dynamic.Core;
using QMS.Core;
using System.ComponentModel.DataAnnotations;
using Furion.Extras.Admin.NET.Entity.Common;

namespace QMS.Application.System
{
    /// <summary>
    /// 人员组织架构
    /// </summary>
    [ApiDescriptionSettings(Name = "SsuEmpOrg", Order = 100)]
    public class SsuEmpService : ISsuEmpService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysOrg> _sysOrgRep;  // 组织机构表仓储
        private readonly IRepository<SysEmp> _sysEmpRep;  // 组织机构表仓储
        private readonly ICacheService _cacheService;
        private readonly IRepository<SysUser> _sysUser;
        private readonly IRepository<SsuProjectUser> _ssuProjectUser;
        private readonly IRepository<SsuGroupUser> _ssuGroupUser;
        private readonly ISysEmpService _sysEmpService;
        private readonly int CacheHours = 12;

        public SsuEmpService(IRepository<SysOrg> sysOrgRep, IRepository<SysEmp> sysEmpRep, ICacheService cacheService, IRepository<SysUser> sysUser,
            ISysEmpService sysEmpService, IRepository<SsuProjectUser> ssuProjectUser, IRepository<SsuGroupUser> ssuGroupUser)
        {
            _sysOrgRep = sysOrgRep;
            _sysEmpRep = sysEmpRep;
            _cacheService = cacheService;
            _sysUser = sysUser;
            _sysEmpService = sysEmpService;
            _ssuProjectUser = ssuProjectUser;
            _ssuGroupUser = ssuGroupUser;
        }

        /// <summary>
        /// (非管理员)获取当前用户数据范围（机构Id）
        /// </summary>
        /// <param name="dataScopes"></param>
        /// <returns></returns>
        private List<long> GetDataScopeList(List<long> dataScopes)
        {
            var dataScopeList = new List<long>();
            // 如果是超级管理员则获取所有组织机构，否则只获取其数据范围的机构数据
            if (!CurrentUserInfo.IsSuperAdmin)
            {
                if (dataScopes.Count < 1)
                    return dataScopeList;

                // 此处获取所有的上级节点，用于构造完整树
                dataScopes.ForEach(u =>
                {
                    var sysOrg = _sysOrgRep.DetachedEntities.FirstOrDefault(c => c.Id == u);
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

        /// <summary>
        /// 获取组织机构列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuEmpOrg/list")]
        public async Task<List<OrgOutput>> GetOrgList([FromQuery] OrgListInput input)
        {
            var dataScopeList = GetDataScopeList(await GetUserDataScopeIdList());

            var pId = !string.IsNullOrEmpty(input.Pid?.Trim());
            var orgs = await _sysOrgRep.DetachedEntities
                                       .Where(pId, u => u.Pid == long.Parse(input.Pid))
                                       .Where(dataScopeList.Count > 0, u => dataScopeList.Contains(u.Id))
                                       .Where(u => u.Status != CommonStatus.DELETED)
                                       .OrderBy(u => u.Sort)
                                       .ProjectToType<OrgOutput>()
                                       .ToListAsync();
            return orgs;
        }

        /// <summary>
        /// 获取组织机构信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuEmpOrg/detail")]
        public async Task<SysOrg> GetOrg([FromQuery] QueryOrgInput input)
        {
            return await _sysOrgRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == long.Parse(input.Id));
        }

        /// <summary>
        /// 根据节点Id获取所有子节点Id集合，包含自己
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<long>> GetChildIdListWithSelfById(long id)
        {
            var childIdList = await _sysOrgRep.DetachedEntities
                                              .Where(u => EF.Functions.Like(u.Pids, $"%{id}%"))
                                              .Select(u => u.Id).ToListAsync();
            childIdList.Add(id);
            return childIdList;
        }

        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SsuEmpOrg/tree")]
        public async Task<dynamic> GetOrgTree()
        {
            var dataScopeList = new List<long>();
            //if (!CurrentUserInfo.IsSuperAdmin)  此处暂时先不考虑显示权限问题，默认所有人均可以显示
            //{
            //    var dataScopes = await GetUserDataScopeIdList();
            //    if (dataScopes.Count < 1)
            //        return dataScopeList;
            //    dataScopeList = GetDataScopeList(dataScopes);
            //}
            var orgs = await _sysOrgRep.DetachedEntities.Where(dataScopeList.Count > 0, u => dataScopeList.Contains(u.Id))
                                                        .Where(u => u.Status == CommonStatus.ENABLE)
                                                        .OrderBy(u => u.Sort)
                                                        .ProjectToType<OrgTreeNode>()
                                                        .ToListAsync();

            return new TreeBuildUtil<OrgTreeNode>().Build(orgs);
        }

        /// <summary>
        /// 获取成员+组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SsuEmpOrg/emp")]
        public async Task<dynamic> GetEmpTree()
        {
            var dataScopeList = new List<long>();
            if (!CurrentUserInfo.IsSuperAdmin)
            {
                var dataScopes = await GetUserDataScopeIdList();
                if (dataScopes.Count < 1)
                    return dataScopeList;
                dataScopeList = GetDataScopeList(dataScopes);
            }
            var orgs = await

                _sysOrgRep.DetachedEntities.Where(dataScopeList.Count > 0, u => dataScopeList.Contains(u.Id))
                                                        .Where(u => u.Status == CommonStatus.ENABLE)
                                                        .OrderBy(u => u.Sort)
                                                        .ProjectToType<OrgTreeNode>()
                                                        .ToListAsync();

            return new TreeBuildUtil<OrgTreeNode>().Build(orgs);
        }

        /// <summary>
        /// 根据数据范围类型获取当前用户的数据范围（机构Id）集合
        /// </summary>
        /// <param name="dataScopeType"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetDataScopeListByDataScopeType(int dataScopeType, long orgId)
        {
            var orgIdList = new List<long>();
            if (orgId < 0)
                return orgIdList;

            // 如果是范围类型是全部数据，则获取当前所有的组织架构Id
            if (dataScopeType == (int)DataScopeType.ALL)
            {
                orgIdList = await _sysOrgRep.DetachedEntities.Where(u => u.Status == CommonStatus.ENABLE).Select(u => u.Id).ToListAsync();
            }
            // 如果范围类型是本部门及以下部门，则查询本节点和子节点集合，包含本节点
            else if (dataScopeType == (int)DataScopeType.DEPT_WITH_CHILD)
            {
                orgIdList = await GetChildIdListWithSelfById(orgId);
            }
            // 如果数据范围是本部门，不含子节点，则直接返回本部门
            else if (dataScopeType == (int)DataScopeType.DEPT)
            {
                orgIdList.Add(orgId);
            }
            return orgIdList;
        }

        /// <summary>
        /// 获取所有的机构组织Id集合
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetAllDataScopeIdList()
        {
            return await _sysOrgRep.DetachedEntities.Select(u => u.Id).ToListAsync();
        }

        /// <summary>
        /// 获取用户数据范围（机构Id集合）
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public async Task<List<long>> GetUserDataScopeIdList()
        {
            return await App.GetService<ISysUserService>().GetUserDataScopeIdList();
        }

        /// <summary>
        /// 根据机构ID获取人员列表
        /// </summary>
        /// <param name="orgInput">机构分页参数</param>
        /// <returns></returns>
        [HttpGet("/SsuEmpOrg/getorguser")]
        public async Task<PageResult<UserOutput>> GetOrgUser([FromQuery] SsuOrgUserInput orgInput)
        {
            PageResult<UserOutput> result = new PageResult<UserOutput>();
            //获取该组织机构下的所有机构
            var orgIds = _sysOrgRep.DetachedEntities.Where(u => u.Pids.Contains("[" + orgInput.orgId.ToString() + "]")).Select(u => u.Id).ToList();
            //机构列表加入自身
            orgIds.Add(orgInput.orgId);
            //获取机构对应页码数的所有人员
            var userIds = _sysEmpRep.DetachedEntities.Where(u => orgIds.Distinct().Contains(u.OrgId)).Select(u => u.Id);
            if (userIds != null && userIds.Any())
            {
                result = _sysUser.DetachedEntities.Where(u => userIds.Contains(u.Id) && u.AdminType != AdminType.SuperAdmin && u.IsDeleted == false)
                    .Select(u => u.Adapt<UserOutput>())
                    .ToADPagedList(orgInput.PageNo, orgInput.PageSize);

                result.Rows.ToList().ForEach(
                    delegate (UserOutput user)
                    {
                        user.SysEmpInfo = _sysEmpService.GetEmpInfo(Convert.ToInt64(user.Id)).Result;
                    }
                    );
            }
            return result;
        }

        /// <summary>
        /// 根据人员ID列表获取人员详细信息列表
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        [HttpPost("/SsuEmpOrg/getuserlist")]
        public async Task<List<SysUser>> GetUserList(IEnumerable<SsuUserInput> userInput)
        {
            List<SysUser> list = new List<SysUser>();
            //先从缓存处取值
            foreach (SsuUserInput input in userInput)
            {
                var cacheUserId = await _cacheService.GetCache<SysUser>(CoreCommonConst.USERID + GetTenantId() + "_" + input.Id);
                var cacheUserName = await _cacheService.GetCache<SysUser>(CoreCommonConst.USERNAME + GetTenantId() + "_" + input.Name);
                if (cacheUserId != null)
                {
                    list.Add(cacheUserId);
                }
                else if (cacheUserName != null)
                {
                    list.Add(cacheUserName);
                }
            }
            //缓存处不存在值则从数据库中取值并缓存在数据库中
            var otherInput = userInput.Where(u => !list.Select(t => t.Id).Contains(u.Id) && !list.Select(t => t.Name).Contains(u.Name) && u.Id != 0);
            if (otherInput == null || otherInput.Count() <= 0)
            {
                return list;
            }

            var otherName = otherInput.Where(u => !string.IsNullOrEmpty(u.Name)).Select(u => u.Name);
            var otherId = otherInput.Where(u => u.Id != 0).Select(u => u.Id);
            var userlist = _sysUser.DetachedEntities
                .Where(otherName.Count() > 0, u => otherName.Contains(u.Name))
                .Where(otherId.Count() > 0, u => otherId.Contains(u.Id))
                .Where(u => u.IsDeleted == false)
                .ToList();
            //针对每个人员ID和名称都做一次缓存，所以此处采用循环的方式
            foreach (SysUser obj in userlist)
            {
                list.Add(obj);
                await _cacheService.SetCacheByHours(CoreCommonConst.USERID + GetTenantId() + "_" + obj.Id, obj, CacheHours);
                await _cacheService.SetCacheByHours(CoreCommonConst.USERNAME + GetTenantId() + "_" + obj.Name, obj, CacheHours);
            }
            return list;
        }

        /// <summary>
        /// 获取租户Id缓存key值
        /// </summary>
        /// <returns></returns>
        private string GetTenantId()
        {
            if (App.User == null) return string.Empty;
            return App.User.FindFirst(ClaimConst.TENANT_ID)?.Value + "_";
        }


        #region 用户查找

        /// <summary>
        /// 根据用户名称模糊查找用户列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("/SsuEmpOrg/getfuzzyusers")]
        public async Task<List<UserOutput>> GetFuzzyUsers([FromQuery] string name)
        {
            return await _sysUser.DetachedEntities.Where(u => u.Name.Contains(name)).Select(u => u.Adapt<UserOutput>()).ToListAsync();
        }

        #endregion 用户查找

        /// <summary>
        /// 根据项目ID和模块ID获取当前指派人列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="modularId"></param>
        /// <returns></returns>
        [HttpGet("/SsuEmpOrg/getresponsibilityuser")]
        public async Task<List<UserOutput>> GetResponsibilityUser([Required][FromQuery] long projectId, [Required][FromQuery] long modularId)
        {
            if (projectId == 0 || modularId == 0)
            {
                throw Oops.Oh("项目ID或模块ID不能为空");
            }
            var cacheProjectModular = _cacheService.GetCache<List<UserOutput>>(CoreCommonConst.PROJECT_MODULAR + projectId + "_" + modularId);
            if (cacheProjectModular.Result != null && cacheProjectModular.Result.Count() > 0)
            {
                return cacheProjectModular.Result;
            }

            var projectUser = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId == projectId).Select(u => u.EmployeeId).ToList();
            //模块人员列表先暂时采用"在人员组上新增模块对应的人员列表，管理员后台维护数据库"，后续有变动再做修改
            //var modularUser = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId == modularId).Select(u => u.EmployeeId).ToList();
            //获取项目人员列表和模块人员列表的交集
            //var userList = projectUser.Intersect(modularUser).ToList();
            var userList = projectUser.ToList();
            var result = await _sysUser.DetachedEntities.Where(u => userList.Contains(u.Id)).Select(u => u.Adapt<UserOutput>()).ToListAsync();
            await _cacheService.SetCache<List<UserOutput>>(CoreCommonConst.PROJECT_MODULAR + projectId + "_" + modularId, result);
            return result;
        }
    }
}