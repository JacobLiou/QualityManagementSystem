using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core;
using QMS.Core.Entity;
using System.Linq.Dynamic.Core;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题操作记录服务
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "SsuIssueOperation", Order = 100)]
    public class SsuIssueOperationService : ISsuIssueOperationService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssueOperation,IssuesDbContextLocator> _ssuIssueOperationRep;

        public SsuIssueOperationService(
            IRepository<SsuIssueOperation,IssuesDbContextLocator> ssuIssueOperationRep
        )
        {
            _ssuIssueOperationRep = ssuIssueOperationRep;
        }

        /// <summary>
        /// 分页查询问题操作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueOperation/page")]
        public async Task<PageResult<SsuIssueOperationOutput>> Page([FromQuery] SsuIssueOperationInput input)
        {
            var ssuIssueOperations = await _ssuIssueOperationRep.DetachedEntities
                                     .Where(u => u.IssueId == input.IssueId)
                                     .Where(u => u.OperationTypeId == input.OperationTypeId)
                                     .Where(!string.IsNullOrEmpty(input.Content), u => u.Content == input.Content)
                                     .Where(u => u.OperationTime == input.OperationTime)
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuIssueOperationInput>(input))
                                     .ProjectToType<SsuIssueOperationOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssueOperations;
        }

        /// <summary>
        /// 增加问题操作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueOperation/add")]
        public async Task Add(AddSsuIssueOperationInput input)
        {
            var ssuIssueOperation = input.Adapt<SsuIssueOperation>();
            await _ssuIssueOperationRep.InsertAsync(ssuIssueOperation);
        }

        /// <summary>
        /// 删除问题操作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueOperation/delete")]
        public async Task Delete(DeleteSsuIssueOperationInput input)
        {
            var ssuIssueOperation = await _ssuIssueOperationRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await _ssuIssueOperationRep.DeleteAsync(ssuIssueOperation);
        }

        /// <summary>
        /// 更新问题操作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueOperation/edit")]
        public async Task Update(UpdateSsuIssueOperationInput input)
        {
            var isExist = await _ssuIssueOperationRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuIssueOperation = input.Adapt<SsuIssueOperation>();
            await _ssuIssueOperationRep.UpdateAsync(ssuIssueOperation,ignoreNullValues:true);
        }

        /// <summary>
        /// 获取问题操作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueOperation/detail")]
        public async Task<SsuIssueOperationOutput> Get([FromQuery] QueryeSsuIssueOperationInput input)
        {
            return (await _ssuIssueOperationRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuIssueOperationOutput>();
        }

        /// <summary>
        /// 获取问题操作记录列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueOperation/list")]
        public async Task<List<SsuIssueOperationOutput>> List([FromQuery] SsuIssueOperationInput input)
        {
            return await _ssuIssueOperationRep.DetachedEntities.ProjectToType<SsuIssueOperationOutput>().ToListAsync();
        }    

    }
}
