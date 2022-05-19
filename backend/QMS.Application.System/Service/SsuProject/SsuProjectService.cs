using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Entity.Common;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    /// <summary>
    /// 项目服务
    /// </summary>
    [ApiDescriptionSettings(Name = "SsuProject", Order = 100)]
    public class SsuProjectService : ISsuProjectService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuProject, MasterDbContextLocator> _ssuProjectRep;
        private readonly IRepository<SsuProjectUser> _ssuProjectUser;
        private readonly IRepository<SysUser> _ssuSysuser;
        private readonly ISysEmpService _sysEmpService;
        private readonly ICacheService<SsuProject> _cacheService;
        private readonly int CacheMinute = 30;

        public SsuProjectService(
            IRepository<SsuProject, MasterDbContextLocator> ssuProjectRep, IRepository<SsuProjectUser> ssuProjectUser, IRepository<SysUser> ssuSysuser,
            ISysEmpService sysEmpService, ICacheService<SsuProject> cacheService
        )
        {
            _ssuProjectRep = ssuProjectRep;
            _ssuProjectUser = ssuProjectUser;
            _ssuSysuser = ssuSysuser;
            _sysEmpService = sysEmpService;
            _cacheService = cacheService;
        }

        /// <summary>
        /// 分页查询项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProject/page")]
        public async Task<PageResult<SsuProjectOutput>> Page([FromQuery] SsuProjectInput input)
        {
            var ssuProjects = await _ssuProjectRep.DetachedEntities
                                     .Where(!string.IsNullOrEmpty(input.ProjectName), u => u.ProjectName == input.ProjectName)
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuProjectInput>(input))
                                     .ProjectToType<SsuProjectOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuProjects;
        }

        /// <summary>
        /// 增加项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuProject/add")]
        public async Task Add(AddSsuProjectInput input)
        {
            var ssuProject = input.Adapt<SsuProject>();
            await _ssuProjectRep.InsertAsync(ssuProject);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuProject/delete")]
        public async Task Delete(DeleteSsuProjectInput input)
        {
            var ssuProject = await _ssuProjectRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await _ssuProjectRep.DeleteAsync(ssuProject);
        }

        /// <summary>
        /// 更新项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuProject/edit")]
        public async Task Update(UpdateSsuProjectInput input)
        {
            var isExist = await _ssuProjectRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuProject = input.Adapt<SsuProject>();
            await _ssuProjectRep.UpdateAsync(ssuProject, ignoreNullValues: true);
            await _cacheService.SetCacheByMinutes(CoreCommonConst.PROJECTID + input.Id, ssuProject, CacheMinute);
        }

        /// <summary>
        /// 获取项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProject/detail")]
        public async Task<SsuProjectOutput> Get([FromQuery] QueryeSsuProjectInput input)
        {
            return (await _ssuProjectRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuProjectOutput>();
        }

        /// <summary>
        /// 问题管理服务调用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuProject/list")]
        public async Task<Dictionary<long, SsuProjectOutput>> List(IEnumerable<long> input)
        {
            return (await _ssuProjectRep.DetachedEntities.Where<SsuProject>(project => input.Contains(project.Id)).ProjectToType<SsuProjectOutput>().ToDictionaryAsync(project => project.Id));
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProject/list")]
        public async Task<List<SsuProjectOutput>> List()
        {
            return await _ssuProjectRep.DetachedEntities.ProjectToType<SsuProjectOutput>().ToListAsync();
        }

        /// <summary>
        /// 根据项目ID获取对应的项目人员
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        [HttpPost("/SsuProject/getprojectuser")]
        public async Task<List<UserOutput>> GetProjectUser(long projectId)
        {
            List<UserOutput> list = new List<UserOutput>();
            var userIds = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId.Equals(projectId)).Select(u => u.EmployeeId);
            if (userIds != null)
            {
                var userList = _ssuSysuser.Where(u => userIds.Contains(u.Id)).ToList();
                if (userList != null)
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
        ///新增人员列表到项目组
        /// </summary>
        /// <param name="projectId">项目组ID</param>
        /// <param name="userIds">人员列表ID</param>
        /// <returns></returns>
        [HttpPost("/SsuProject/insertprojectgroup")]
        public async Task InsertProjectGroup(long projectId, long[] userIds)
        {
            List<SsuProjectUser> list = new List<SsuProjectUser>();
            var resultList = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId.Equals(projectId)).Select(u => u.EmployeeId);
            foreach (long employeeId in userIds)
            {
                if (!resultList.Contains(employeeId))
                {
                    SsuProjectUser projectUser = new SsuProjectUser();
                    projectUser.ProjectId = projectId;
                    projectUser.EmployeeId = employeeId;
                    list.Add(projectUser);
                }
            }

            if (list != null && list.Count > 0)
            {
                await _ssuProjectUser.InsertAsync(list);
            }
        }

        /// <summary>
        /// 根据项目ID列表获取项目详细信息列表
        /// </summary>
        /// <param name="projectIds"></param>
        /// <returns></returns>
        [HttpPost("/SsuProject/getprojectlist")]
        public async Task<Dictionary<long, SsuProject>> GetProjectList(IEnumerable<long> projectIds)
        {
            Dictionary<long, SsuProject> Dcit = new Dictionary<long, SsuProject>();
            var products  = _ssuProjectRep.DetachedEntities.Where(u => projectIds.Contains(u.Id)).ToDictionary(u => u.Id);
            //针对每个产品ID都做一次缓存，所以此处采用循环的方式
            foreach (SsuProject obj in products.Values)
            {
                var cacheProduct = _cacheService.GetCache(CoreCommonConst.PROJECTID + obj.Id);
                if (cacheProduct.Result != null)
                {
                    Dcit.Add(obj.Id, cacheProduct.Result);
                }
                else
                {
                    Dcit.Add(obj.Id, obj);
                    await _cacheService.SetCacheByMinutes(CoreCommonConst.PROJECTID + obj.Id, obj, CacheMinute);
                }
            }
            return Dcit;
        }
    }
}