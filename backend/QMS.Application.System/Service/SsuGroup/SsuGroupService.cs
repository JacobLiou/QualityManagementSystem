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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    /// <summary>
    /// 人员组服务
    /// </summary>
    [ApiDescriptionSettings(Name = "SsuGroup", Order = 100)]
    public class SsuGroupService : ISsuGroupService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuGroup, MasterDbContextLocator> _ssuGroupRep;
        private readonly IRepository<SsuGroupUser> _ssuGroupUser;
        private readonly IRepository<SysUser> _ssuSysuser;
        private readonly ISysEmpService _sysEmpService;

        public SsuGroupService(
            IRepository<SsuGroup, MasterDbContextLocator> ssuGroupRep, IRepository<SsuGroupUser> ssuGroupUser, IRepository<SysUser> ssuSysuser, ISysEmpService sysEmpService
        )
        {
            _ssuGroupRep = ssuGroupRep;
            _ssuGroupUser = ssuGroupUser;
            _ssuSysuser = ssuSysuser;
            _sysEmpService = sysEmpService;
        }

        /// <summary>
        /// 分页查询人员组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/ssuGroup/page")]
        public async Task<PageResult<SsuGroupOutput>> Page([FromQuery] SsuGroupInput input)
        {
            var ssuGroups = await _ssuGroupRep.DetachedEntities
                                     //.Where(!string.IsNullOrEmpty(input.GroupName), u => u.GroupName == input.GroupName)
                                     .Where(!string.IsNullOrEmpty(input.GroupName), u => u.GroupName.Contains(input.GroupName))
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuGroupInput>(input))
                                     .ProjectToType<SsuGroupOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            //获取人员组的关联人员列表
            foreach (SsuGroupOutput output in ssuGroups.Rows)
            {
                var userList = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId == output.Id).Select(u => u.EmployeeId).ToList();
                if (userList != null && userList.Count > 0)
                {
                    output.UserList = _ssuSysuser.DetachedEntities.Where(u => userList.Contains(u.Id)).ToList().Adapt<List<UserOutput>>();
                }
            }
            return ssuGroups;
        }

        /// <summary>
        /// 增加人员组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuGroup/add")]
        public async Task Add(AddSsuGroupInput input)
        {
            var ssuGroup = input.Adapt<SsuGroup>();
            var result = await _ssuGroupRep.InsertAsync(ssuGroup);
            if (result != null && result.Entity.Id != 0 && input.UserIdList.Count() > 0)
            {
                var list = input.UserIdList.Select(u => new SsuGroupUser() { GroupId = result.Entity.Id, EmployeeId = u });
                await _ssuGroupUser.InsertAsync(list);
            }
        }

        /// <summary>
        /// 删除人员组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuGroup/delete")]
        public async Task Delete(DeleteSsuGroupInput input)
        {
            var ssuGroup = await _ssuGroupRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await _ssuGroupRep.DeleteAsync(ssuGroup);

            var ssuGroupUser = await _ssuGroupUser.DetachedEntities.FirstOrDefaultAsync(u => u.GroupId == input.Id);
            if (ssuGroupUser != null)
            {
                await _ssuGroupUser.DeleteAsync(ssuGroupUser);
            }
        }

        /// <summary>
        /// 更新人员组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuGroup/edit")]
        public async Task Update(UpdateSsuGroupInput input)
        {
            var isExist = await _ssuGroupRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh("人员组不存在");

            var ssuGroup = input.Adapt<SsuGroup>();
            await _ssuGroupRep.UpdateAsync(ssuGroup, ignoreNullValues: true);

            //更新人员组人员列表
            var existsList = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId.Equals(input.Id)).Select(u => u.EmployeeId);
            //获取在新增列表中存在，但是不存在于人员组中的ID，执行新增操作
            var intersectionList = input.UserIdList.Except(existsList).Select(u => new SsuGroupUser() { GroupId = input.Id, EmployeeId = u }); ;
            if (intersectionList != null && intersectionList.Count() > 0)
            {
                await _ssuGroupUser.InsertAsync(intersectionList);
            }

            //获取在人员组中存在的ID，但是不存在于新增的ID列表中的ID，执行删除操作
            var differenceList = existsList.Except(input.UserIdList);
            var ssuGroupUser = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId == input.Id && differenceList.Contains(u.EmployeeId));
            if (ssuGroupUser != null && ssuGroupUser.Count() > 0)
            {
                await _ssuGroupUser.DeleteAsync(ssuGroupUser);
            }
        }

        /// <summary>
        /// 获取人员组明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuGroup/detail")]
        public async Task<SsuGroupOutput> Get([FromQuery] QueryeSsuGroupInput input)
        {
            var detail = await _ssuGroupRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (detail == null)
            {
                throw Oops.Oh("人员组不存在");
            }
            var result = detail.Adapt<SsuGroupOutput>();
            var groupUserId = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId == input.Id)
                .Select(u => u.EmployeeId).ToList();
            result.UserList = _ssuSysuser.DetachedEntities.Where(u => groupUserId.Contains(u.Id)).Adapt<List<UserOutput>>();
            return result;
        }

        /// <summary>
        /// 获取人员组列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SsuGroup/select")]
        public async Task<List<SsuGroupOutput>> Select()
        {
            var result = await _ssuGroupRep.DetachedEntities.ProjectToType<SsuGroupOutput>().ToListAsync();
            foreach (SsuGroupOutput output in result)
            {
                var userList = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId == output.Id).Select(u => u.EmployeeId).ToList();
                if (userList != null && userList.Count > 0)
                {
                    output.UserList = _ssuSysuser.DetachedEntities.Where(u => userList.Contains(u.Id)).ToList().Adapt<List<UserOutput>>();
                }
            }
            return result;
        }

        /// <summary>
        ///根据人员组ID获取对应的人员列表
        /// </summary>
        /// <param name="groupInput">人员组分页参数</param>
        /// <returns></returns>
        [HttpGet("/SsuGroup/getgroupusers")]
        public async Task<PageResult<UserOutput>> GetGroupUsers([FromQuery] SsuGroupUserInput groupInput)
        {
            PageResult<UserOutput> result = new PageResult<UserOutput>();
            var userIds = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId.Equals(groupInput.groupId)).Select(u => u.EmployeeId);
            if (userIds != null && userIds.Any())
            {
                result = _ssuSysuser.DetachedEntities.Where(u => userIds.Contains(u.Id) && u.AdminType != AdminType.SuperAdmin && u.IsDeleted == false)
                    .Select(u => u.Adapt<UserOutput>())
                    .ToADPagedList(groupInput.PageNo, groupInput.PageSize);
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
        ///新增人员列表到人员组
        /// </summary>
        /// <param name="groupId">人员组ID</param>
        /// <param name="userIds">人员列表ID</param>
        /// <returns></returns>
        [HttpPost("/SsuGroup/insertusergroup")]
        public async Task InsertUserGroup(long groupId, IEnumerable<long> userIds)
        {
            List<SsuGroupUser> list = new List<SsuGroupUser>();
            var existsList = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId.Equals(groupId)).Select(u => u.EmployeeId);
            //获取在新增列表中存在，但是不存在于人员组中的ID，执行新增操作
            var intersectionList = userIds.Except(existsList);
            foreach (long employeeId in intersectionList)
            {
                SsuGroupUser groupUser = new SsuGroupUser();
                groupUser.GroupId = groupId;
                groupUser.EmployeeId = employeeId;
                list.Add(groupUser);
            }
            if (list != null && list.Count > 0)
            {
                await _ssuGroupUser.InsertAsync(list);
            }

            //获取在人员组中存在的ID，但是不存在于新增的ID列表中的ID，执行删除操作
            var differenceList = existsList.Except(userIds);
            if (differenceList != null && differenceList.Count() > 0)
            {
                foreach (long employeeId in differenceList)
                {
                    await _ssuGroupUser.DeleteAsync(employeeId);
                }
            }
        }
    }
}