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
        private readonly ICacheService _cacheService;
        private readonly int CacheMinute = 30;

        public SsuProjectService(
            IRepository<SsuProject, MasterDbContextLocator> ssuProjectRep, IRepository<SsuProjectUser> ssuProjectUser, IRepository<SysUser> ssuSysuser,
            ISysEmpService sysEmpService, ICacheService cacheService
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
                                     //.Where(!string.IsNullOrEmpty(input.ProjectName), u => u.ProjectName == input.ProjectName)
                                     .Where(!string.IsNullOrEmpty(input.ProjectName), u => u.ProjectName.Contains(input.ProjectName))
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuProjectInput>(input))
                                     .ProjectToType<SsuProjectOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);
            //获取项目的关联人员列表
            foreach (SsuProjectOutput output in ssuProjects.Rows)
            {
                var userList = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId == output.Id).Select(u => u.EmployeeId).ToList();
                if (userList != null && userList.Count > 0)
                {
                    output.UserList = _ssuSysuser.DetachedEntities.Where(u => userList.Contains(u.Id)).ToList().Adapt<List<UserOutput>>();
                }
            }
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
            var result = await _ssuProjectRep.InsertAsync(ssuProject);

            if (result != null && result.Entity.Id != 0 && input.UserIdList.Count() > 0)
            {
                var list = input.UserIdList.Select(u => new SsuProjectUser() { ProjectId = result.Entity.Id, EmployeeId = u });
                await _ssuProjectUser.InsertAsync(list);
            }
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

            var ssuProjectUser = await _ssuProjectUser.DetachedEntities.FirstOrDefaultAsync(u => u.ProjectId == input.Id);
            if (ssuProjectUser != null)
            {
                await _ssuProjectUser.DeleteAsync(ssuProjectUser);
            }
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
            if (!isExist) throw Oops.Oh("项目组不存在");

            var ssuProject = input.Adapt<SsuProject>();
            await _ssuProjectRep.UpdateAsync(ssuProject, ignoreNullValues: true);
            await _cacheService.SetCacheByMinutes(CoreCommonConst.PROJECTID + input.Id, ssuProject, CacheMinute);


            //更新项目组人员列表
            var existsList = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId.Equals(input.Id)).Select(u => u.EmployeeId);
            //获取在新增列表中存在，但是不存在于人员组中的ID，执行新增操作
            var intersectionList = input.UserIdList.Except(existsList).Select(u => new SsuProjectUser() { ProjectId = input.Id, EmployeeId = u }); ;
            if (intersectionList != null && intersectionList.Count() > 0)
            {
                await _ssuProjectUser.InsertAsync(intersectionList);
            }

            //获取在项目组中存在的ID，但是不存在于新增的ID列表中的ID，执行删除操作
            var differenceList = existsList.Except(input.UserIdList);
            var ssuProjectUser = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId == input.Id && differenceList.Contains(u.EmployeeId));
            if (ssuProjectUser != null && ssuProjectUser.Count() > 0)
            {
                await _ssuProjectUser.DeleteAsync(ssuProjectUser);
            }
        }

        /// <summary>
        /// 获取项目明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProject/detail")]
        public async Task<SsuProjectOutput> Get([FromQuery] QueryeSsuProjectInput input)
        {
            var detail = await _ssuProjectRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (detail == null)
            {
                throw Oops.Oh("项目不存在");
            }
            var result = detail.Adapt<SsuProjectOutput>();
            var projectUserId = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId == input.Id)
                .Select(u => u.EmployeeId).ToList();
            result.UserList = _ssuSysuser.DetachedEntities.Where(u => projectUserId.Contains(u.Id)).Adapt<List<UserOutput>>();
            return result;
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
        [HttpGet("/SsuProject/select")]
        public async Task<List<SsuProjectOutput>> Select()
        {
            var result = await _ssuProjectRep.DetachedEntities.ProjectToType<SsuProjectOutput>().ToListAsync();
            foreach (SsuProjectOutput output in result)
            {
                var userList = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId == output.Id).Select(u => u.EmployeeId).ToList();
                if (userList != null && userList.Count > 0)
                {
                    output.UserList = _ssuSysuser.DetachedEntities.Where(u => userList.Contains(u.Id)).ToList().Adapt<List<UserOutput>>();
                }
            }
            return result;
        }

        /// <summary>
        /// 根据项目ID获取对应的项目人员
        /// </summary>
        /// <param name="projectInput">项目分页参数</param>
        /// <returns></returns>
        [HttpGet("/SsuProject/getprojectuser")]
        public async Task<PageResult<UserOutput>> GetProjectUser([FromQuery] SsuProjectUserInput projectInput)
        {
            PageResult<UserOutput> result = new PageResult<UserOutput>();
            var userIds = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId.Equals(projectInput.projectId)).Select(u => u.EmployeeId);
            if (userIds != null && userIds.Any())
            {
                result = _ssuSysuser.DetachedEntities.Where(u => userIds.Contains(u.Id) && u.AdminType != AdminType.SuperAdmin && u.IsDeleted == false)
                    .Select(u => u.Adapt<UserOutput>())
                    .ToADPagedList(projectInput.PageNo, projectInput.PageSize);

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
        ///新增人员列表到项目组
        /// </summary>
        /// <param name="projectId">项目组ID</param>
        /// <param name="userIds">人员列表ID</param>
        /// <returns></returns>
        [HttpPost("/SsuProject/insertprojectgroup")]
        public async Task InsertProjectGroup(long projectId, IEnumerable<long> userIds)
        {
            List<SsuProjectUser> Addlist = new List<SsuProjectUser>();
            var existsList = _ssuProjectUser.DetachedEntities.Where(u => u.ProjectId.Equals(projectId)).Select(u => u.EmployeeId);
            //获取在新增列表中存在，但是不存在于项目组中的ID
            var intersectionList = userIds.Except(existsList);
            foreach (long employeeId in intersectionList)
            {
                SsuProjectUser projectUser = new SsuProjectUser();
                projectUser.ProjectId = projectId;
                projectUser.EmployeeId = employeeId;
                Addlist.Add(projectUser);
            }
            if (Addlist != null && Addlist.Count > 0)
            {
                await _ssuProjectUser.InsertAsync(Addlist);
            }

            //获取在项目组中存在的ID，但是不存在于新增的ID列表中的ID
            var differenceList = existsList.Except(userIds);
            if (differenceList != null && differenceList.Count() > 0)
            {
                foreach (long employeeId in differenceList)
                {
                    await _ssuProjectUser.DeleteAsync(employeeId);
                }
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
            var products = _ssuProjectRep.DetachedEntities.Where(u => projectIds.Contains(u.Id) && u.IsDeleted == false).ToDictionary(u => u.Id);
            //针对每个产品ID都做一次缓存，所以此处采用循环的方式
            foreach (SsuProject obj in products.Values)
            {
                var cacheProduct = _cacheService.GetCache<SsuProject>(CoreCommonConst.PROJECTID + obj.Id);
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