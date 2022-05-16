using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Entity.Common;
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
    [ApiDescriptionSettings( Name = "SsuGroup", Order = 100)]
    public class SsuGroupService : ISsuGroupService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuGroup, MasterDbContextLocator> _ssuGroupRep;

        public SsuGroupService(
            IRepository<SsuGroup, MasterDbContextLocator> ssuGroupRep
        )
        {
            _ssuGroupRep = ssuGroupRep;
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
                                     .Where(!string.IsNullOrEmpty(input.GroupName), u => u.GroupName == input.GroupName)
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
        public async Task<List<SsuGroupOutput>> List([FromQuery] SsuGroupInput input)
        {
            return await _ssuGroupRep.DetachedEntities.ProjectToType<SsuGroupOutput>().ToListAsync();
        }
    }
}