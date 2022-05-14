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
    /// 项目服务
    /// </summary>
    [ApiDescriptionSettings(Name = "SsuProject", Order = 100)]
    public class SsuProjectService : ISsuProjectService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuProject,MasterDbContextLocator> _ssuProjectRep;

        public SsuProjectService(
            IRepository<SsuProject,MasterDbContextLocator> ssuProjectRep
        )
        {
            _ssuProjectRep = ssuProjectRep;
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
            await _ssuProjectRep.UpdateAsync(ssuProject,ignoreNullValues:true);
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
        /// 获取项目列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProject/list")]
        public async Task<List<SsuProjectOutput>> List([FromQuery] SsuProjectInput input)
        {
            return await _ssuProjectRep.DetachedEntities.ProjectToType<SsuProjectOutput>().ToListAsync();
        }    

    }
}
