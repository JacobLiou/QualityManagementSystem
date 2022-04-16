using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题管理服务
    /// </summary>
    [ApiDescriptionSettings("自己的业务", Name = "SsuIssues", Order = 100)]
    public class SsuIssuesService : ISsuIssuesService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssues,IssuesDbContextLocator> _ssuIssuesRep;

        public SsuIssuesService(
            IRepository<SsuIssues,IssuesDbContextLocator> ssuIssuesRep
        )
        {
            _ssuIssuesRep = ssuIssuesRep;
        }

        /// <summary>
        /// 分页查询问题管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssues/page")]
        public async Task<PageResult<SsuIssuesOutput>> Page([FromQuery] SsuIssuesInput input)
        {
            var ssuIssuess = await _ssuIssuesRep.DetachedEntities
                                     .Where(!string.IsNullOrEmpty(input.Title), u => u.Title == input.Title)
                                     .Where(!string.IsNullOrEmpty(input.Description), u => u.Description == input.Description)
                                     .Where(u => u.Status == input.Status)
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuIssuesInput>(input))
                                     .ProjectToType<SsuIssuesOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        /// <summary>
        /// 增加问题管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssues/add")]
        public async Task Add(AddSsuIssuesInput input)
        {
            var ssuIssues = input.Adapt<SsuIssues>();
            await _ssuIssuesRep.InsertAsync(ssuIssues);
        }

        /// <summary>
        /// 删除问题管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssues/delete")]
        public async Task Delete(DeleteSsuIssuesInput input)
        {
            var ssuIssues = await _ssuIssuesRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await _ssuIssuesRep.DeleteAsync(ssuIssues);
        }

        /// <summary>
        /// 更新问题管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssues/edit")]
        public async Task Update(UpdateSsuIssuesInput input)
        {
            var isExist = await _ssuIssuesRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuIssues = input.Adapt<SsuIssues>();
            await _ssuIssuesRep.UpdateAsync(ssuIssues,ignoreNullValues:true);
        }

        /// <summary>
        /// 获取问题管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssues/detail")]
        public async Task<SsuIssuesOutput> Get([FromQuery] QueryeSsuIssuesInput input)
        {
            return (await _ssuIssuesRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuIssuesOutput>();
        }

        /// <summary>
        /// 获取问题管理列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssues/list")]
        public async Task<List<SsuIssuesOutput>> List([FromQuery] SsuIssuesInput input)
        {
            return await _ssuIssuesRep.DetachedEntities.ProjectToType<SsuIssuesOutput>().ToListAsync();
        }    

    }
}
