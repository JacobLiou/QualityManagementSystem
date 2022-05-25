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
        private readonly ICacheService<SysUser> _cacheService;
        private readonly IRepository<SysUser> _sysUser;
        private readonly ISysEmpService _sysEmpService;
        private readonly int CacheMinute = 30;

        public SsuEmpService(IRepository<SysOrg> sysOrgRep, IRepository<SysEmp> sysEmpRep, ICacheService<SysUser> cacheService, IRepository<SysUser> sysUser, ISysEmpService sysEmpService)
        {
            _sysOrgRep = sysOrgRep;
            _sysEmpRep = sysEmpRep;
            _cacheService = cacheService;
            _sysUser = sysUser;
            _sysEmpService = sysEmpService;
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
            if (!CurrentUserInfo.IsSuperAdmin)
            {
                var dataScopes = await GetUserDataScopeIdList();
                if (dataScopes.Count < 1)
                    return dataScopeList;
                dataScopeList = GetDataScopeList(dataScopes);
            }
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
        /// <param name="orgId"></param>
        /// <returns></returns>
        [HttpPost("/SsuEmpOrg/getorguser")]
        public async Task<List<UserOutput>> GetOrgUser(long orgId)
        {
            List<UserOutput> list = new List<UserOutput>();
            //获取该组织机构下的所有机构
            var orgIds = _sysOrgRep.DetachedEntities.Where(u => u.Pids.Contains(orgId.ToString())).Select(u => u.Id).ToList();
            //机构列表加入自身
            orgIds.Add(orgId);
            //获取机构对应的所有人员
            var userIds = _sysEmpRep.DetachedEntities.Where(u => orgIds.Distinct().Contains(u.OrgId)).Select(u => u.Id).ToList();
            if (userIds != null && userIds.Count > 0)
            {
                var userList = _sysUser.Where(u => userIds.Contains(u.Id) && u.AdminType != AdminType.SuperAdmin).ToList();
                if (userList != null && userList.Count > 0)
                {
                    foreach (SysUser user in userList)
                    {
                        UserOutput output = user.Adapt<UserOutput>();
                        output.SysEmpInfo = await _sysEmpService.GetEmpInfo(user.Id);
                        list.Add(output);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 根据人员ID列表获取人员详细信息列表
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpPost("/SsuEmpOrg/getuserlist")]
        public async Task<Dictionary<long, SysUser>> GetUserList(IEnumerable<long> userIds)
        {
            Dictionary<long, SysUser> Dcit = new Dictionary<long, SysUser>();
            var products = _sysUser.DetachedEntities.Where(u => userIds.Contains(u.Id) && u.IsDeleted == false).ToDictionary(u => u.Id);
            //针对每个产品ID都做一次缓存，所以此处采用循环的方式
            foreach (SysUser obj in products.Values)
            {
                var cacheProduct = _cacheService.GetCache(CoreCommonConst.USERID + obj.Id);
                if (cacheProduct.Result != null)
                {
                    Dcit.Add(obj.Id, cacheProduct.Result);
                }
                else
                {
                    Dcit.Add(obj.Id, obj);
                    await _cacheService.SetCacheByMinutes(CoreCommonConst.USERID + obj.Id, obj, CacheMinute);
                }
            }
            return Dcit;
        }
    }
}