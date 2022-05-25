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
            await _ssuGroupRep.InsertAsync(ssuGroup);
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
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuGroup = input.Adapt<SsuGroup>();
            await _ssuGroupRep.UpdateAsync(ssuGroup, ignoreNullValues: true);
        }

        /// <summary>
        /// 获取人员组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuGroup/detail")]
        public async Task<SsuGroupOutput> Get([FromQuery] QueryeSsuGroupInput input)
        {
            return (await _ssuGroupRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuGroupOutput>();
        }

        /// <summary>
        /// 获取人员组列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuGroup/list")]
        public async Task<List<SsuGroupOutput>> List()
        {
            return await _ssuGroupRep.DetachedEntities.ProjectToType<SsuGroupOutput>().ToListAsync();
        }

        /// <summary>
        ///根据人员组ID获取对应的人员列表
        /// </summary>
        /// <param name="groupId">人员组ID</param>
        /// <returns></returns>
        [HttpPost("/SsuGroup/getgroupusers")]
        public async Task<List<UserOutput>> GetGroupUsers(long groupId)
        {
            List<UserOutput> list = new List<UserOutput>();
            var userIds = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId.Equals(groupId)).Select(u => u.EmployeeId);
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
        ///新增人员列表到人员组
        /// </summary>
        /// <param name="groupId">人员组ID</param>
        /// <param name="userIds">人员列表ID</param>
        /// <returns></returns>
        [HttpPost("/SsuGroup/insertusergroup")]
        public async Task InsertUserGroup(long groupId, IEnumerable<long> userIds)
        {
            List<SsuGroupUser> list = new List<SsuGroupUser>();
            var resultList = _ssuGroupUser.DetachedEntities.Where(u => u.GroupId.Equals(groupId)).Select(u => u.EmployeeId);
            foreach (long employeeId in userIds)
            {
                if (!resultList.Contains(employeeId))
                {
                    SsuGroupUser groupUser = new SsuGroupUser();
                    groupUser.GroupId = groupId;
                    groupUser.EmployeeId = employeeId;
                    list.Add(groupUser);
                }
            }
            if (list != null && list.Count > 0)
            {
                await _ssuGroupUser.InsertAsync(list);
            }
        }
    }
}