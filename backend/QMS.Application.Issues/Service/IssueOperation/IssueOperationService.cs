using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
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
    [ApiDescriptionSettings("问题管理服务", Name = "IssueOperation", Order = 100)]
    public class IssueOperationService : IIssueOperationService, IDynamicApiController, ITransient
    {
        private readonly IRepository<IssueOperation, IssuesDbContextLocator> _issueOperationRep;

        public IssueOperationService(
            IRepository<IssueOperation, IssuesDbContextLocator> issueOperationRep
        )
        {
            _issueOperationRep = issueOperationRep;
        }

        /// <summary>
        /// 分页查询问题操作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[HttpPost("/issue/operation/page")]
        //public async Task<PageResult<IssueOperationOutput>> Page(IssueOperationInput input)
        //{
        //    var issueOperations = await _issueOperationRep.DetachedEntities
        //                             .Where(u => u.IssueId == input.IssueId)
        //                             //.Where(u => u.OperationTypeId == input.OperationTypeId)
        //                             //.Where(!string.IsNullOrEmpty(input.Content), u => u.Content == input.Content)
        //                             //.Where(u => u.OperationTime == input.OperationTime)
        //                             .OrderBy(PageInputOrder.OrderBuilder(input))
        //                             .ProjectToType<IssueOperationOutput>()
        //                             .ToADPagedListAsync(input.PageNo, input.PageSize);

        //    return issueOperations;
        //}

        public class IssueIdModel
        {
            public long IssueId { get; set; }
        }

        /// <summary>
        /// 查询问题操作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/operation/page")]
        public async Task<PageResult<IssueOperationOutput>> Page(IssueIdModel input)
        {
            Helper.Helper.CheckInput(input);

            var issueOperations = await _issueOperationRep.DetachedEntities
                                     .Where(u => u.IssueId == input.IssueId)
                                     .ProjectToType<IssueOperationOutput>().ToListAsync();

            return new PageResult<IssueOperationOutput>
            {
                PageNo = 1,
                PageSize = issueOperations.Count,
                TotalRows = issueOperations.Count,
                Rows = issueOperations,
                TotalPage = 1,
            };
        }

        ///// <summary>
        ///// 增加问题操作记录
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost("/IssueOperation/add")]
        //public async Task Add(AddIssueOperationInput input)
        //{
        //    var issueOperation = input.Adapt<IssueOperation>();
        //    await _issueOperationRep.InsertAsync(issueOperation);
        //}

        ///// <summary>
        ///// 删除问题操作记录
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost("/IssueOperation/delete")]
        //public async Task Delete(DeleteIssueOperationInput input)
        //{
        //    var issueOperation = await _issueOperationRep.FirstOrDefaultAsync(u => u.Id == input.Id);
        //    await _issueOperationRep.DeleteAsync(issueOperation);
        //}

        ///// <summary>
        ///// 更新问题操作记录
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost("/IssueOperation/edit")]
        //public async Task Update(UpdateIssueOperationInput input)
        //{
        //    var isExist = await _issueOperationRep.AnyAsync(u => u.Id == input.Id, false);
        //    if (!isExist) throw Oops.Oh(ErrorCode.D3000);

        //    var issueOperation = input.Adapt<IssueOperation>();
        //    await _issueOperationRep.UpdateAsync(issueOperation,ignoreNullValues:true);
        //}

        ///// <summary>
        ///// 获取问题操作记录
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpGet("/IssueOperation/detail")]
        //public async Task<IssueOperationOutput> Get([FromQuery] QueryeIssueOperationInput input)
        //{
        //    return (await _issueOperationRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<IssueOperationOutput>();
        //}

        ///// <summary>
        ///// 获取问题操作记录列表
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpGet("/IssueOperation/list")]
        //public async Task<List<IssueOperationOutput>> List([FromQuery] IssueOperationInput input)
        //{
        //    return await _issueOperationRep.DetachedEntities.ProjectToType<IssueOperationOutput>().ToListAsync();
        //}    

    }
}
