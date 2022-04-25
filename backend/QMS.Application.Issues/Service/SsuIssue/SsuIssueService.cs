using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QMS.Application.Issues.IssueService.Dto.QueryList;
using QMS.Application.Issues.Service.SsuIssue.Dto.Add;
using QMS.Application.Issues.Service.SsuIssue.Dto.Update;
using QMS.Core;
using QMS.Core.Entity;
using System.Linq.Dynamic.Core;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题记录服务
    /// </summary>
    [ApiDescriptionSettings("自己的业务", Name = "SsuIssue", Order = 100)]
    public class SsuIssueService : ISsuIssueService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssue, IssuesDbContextLocator> _ssuIssueRep;
        private readonly IRepository<SsuIssueDetail, IssuesDbContextLocator> _ssuIssueDetailRep;

        public SsuIssueService(
            IRepository<SsuIssue, IssuesDbContextLocator> ssuIssueRep,
            IRepository<SsuIssueDetail, IssuesDbContextLocator> ssuIssueDetailRep
        )
        {
            this._ssuIssueRep = ssuIssueRep;
            this._ssuIssueDetailRep = ssuIssueDetailRep;
        }

        /// <summary>
        /// 增加问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/add")]
        public async Task Add(InIssue input)
        {
            var ssuIssue = input.Adapt<SsuIssue>();
            ssuIssue.CreateTime = DateTime.Now;
            ssuIssue.CreatorId = Helper.Helper.GetCurrentUser();

            EntityEntry<SsuIssue> issue = await this._ssuIssueRep.InsertAsync(ssuIssue, ignoreNullValues: true).ConfigureAwait(false);

            var detail = input.Adapt<SsuIssueDetail>();
            detail.Id = issue.Entity.Id;

            await this._ssuIssueDetailRep.InsertAsync(detail, ignoreNullValues: true);
        }

        /// <summary>
        /// 删除问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/delete")]
        public async Task Delete(DeleteSsuIssueInput input)
        {
            var ssuIssue = await _ssuIssueRep.FirstOrDefaultAsync(u => u.Id == input.Id);

            if (!ssuIssue.IsDeleted)
            {
                ssuIssue.IsDeleted = true;

                await this._ssuIssueRep.UpdateAsync(ssuIssue);
            }
            else
            {
                await this._ssuIssueRep.DeleteAsync(ssuIssue);
            }
        }

        /// <summary>
        /// 更新问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/edit")]
        public async Task Update(UpdateSsuIssueInput input)
        {
            var isExist = await _ssuIssueRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuIssue = input.Adapt<SsuIssue>();
            await _ssuIssueRep.UpdateAsync(ssuIssue, ignoreNullValues: true);
        }

        [HttpGet("/SsuIssue/detail")]
        public async Task<OutputDetailIssue> Get([FromQuery] BaseId input)
        {
            OutputDetailIssue issue = (await this._ssuIssueDetailRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<OutputDetailIssue>();

            return issue;
        }

        /// <summary>
        /// 获取问题记录列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssue/list")]
        public async Task<List<SsuIssueOutput>> List([FromQuery] SsuIssueInput input)
        {
            return await _ssuIssueRep.DetachedEntities.ProjectToType<SsuIssueOutput>().ToListAsync();
        }

        [HttpPost("/SsuIssue/execute")]
        public async Task Update(InSolve input)
        {
            var common = input.Adapt<SsuIssue>();
            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);
        }

        [HttpPost("/SsuIssue/validate")]
        public async Task Update(InValidate input)
        {
            var common = input.Adapt<SsuIssue>();
            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);
        }

        [HttpPost("/SsuIssue/hangup")]
        public async Task Update(InHangup input)
        {
            var common = input.Adapt<SsuIssue>();
            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);
        }

        [HttpPost("/SsuIssue/redispatch")]
        public async Task Update(InReDispatch input)
        {
            var common = input.Adapt<SsuIssue>();
            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);
        }

        [HttpGet("/SsuIssue/page")]
        public async Task<PageResult<OutputGeneralIssue>> PageWithGeneralCondition([FromQuery] BaseQueryModel input)
        {
            var ssuIssues = await this._ssuIssueRep.DetachedEntities
                                     .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                     .Where(input.Module != null, u => u.Module == input.Module)
                                     .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                     .Where(input.Status != null, u => u.Status == input.Status)
                                     .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                     .OrderBy(PageInputOrder.OrderBuilder<BaseQueryModel>(input))
                                     .ProjectToType<OutputGeneralIssue>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssues;
        }

        [HttpGet("/SsuIssue/page-general-creator")]
        public async Task<PageResult<OutputGeneralIssue>> PageByCreator([FromQuery] QueryListByCreator input)
        {
            var ssuIssuess = await this._ssuIssueRep.DetachedEntities
                                     .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                     .Where(input.Module != null, u => u.Module == input.Module)
                                     .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                     .Where(input.Status != null, u => u.Status == input.Status)
                                     .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                     .Where(input.CreatorId > 0, u => u.CreatorId == input.CreatorId)
                                     .OrderBy(PageInputOrder.OrderBuilder<QueryListByCreator>(input))
                                     .ProjectToType<OutputGeneralIssue>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet("/SsuIssue/page-general-dispatcher")]
        public async Task<PageResult<OutputGeneralIssue>> PageByDispatcher([FromQuery] QueryListByDispatcher input)
        {
            var ssuIssuess = await this._ssuIssueRep.DetachedEntities
                                    .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                    .Where(input.Module != null, u => u.Module == input.Module)
                                    .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                    .Where(input.Status != null, u => u.Status == input.Status)
                                    .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                    .Where(input.Dispatcher > 0, u => u.Dispatcher == input.Dispatcher)
                                    .OrderBy(PageInputOrder.OrderBuilder<QueryListByDispatcher>(input))
                                    .ProjectToType<OutputGeneralIssue>()
                                    .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet("/SsuIssue/page-general-executor")]
        public async Task<PageResult<OutputGeneralIssue>> PageByExector([FromQuery] QueryListByExecutor input)
        {
            var ssuIssuess = await this._ssuIssueRep.DetachedEntities
                                   .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                   .Where(input.Module != null, u => u.Module == input.Module)
                                   .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                   .Where(input.Status != null, u => u.Status == input.Status)
                                   .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                   .Where(input.Executor > 0, u => u.Dispatcher == input.Executor)
                                    .OrderBy(PageInputOrder.OrderBuilder<QueryListByExecutor>(input))
                                   .ProjectToType<OutputGeneralIssue>()
                                   .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet("/SsuIssue/page-general-solved")]
        public async Task<PageResult<OutputGeneralIssue>> PageBySolved([FromQuery] QueryListInSolved input)
        {
            var ssuIssuess = await this._ssuIssueRep.DetachedEntities
                                   .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                   .Where(input.Module != null, u => u.Module == input.Module)
                                   .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                   .Where(input.Status != null, u => (int)u.Status == input.Status)
                                   .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                    .OrderBy(PageInputOrder.OrderBuilder<QueryListInSolved>(input))
                                   .ProjectToType<OutputGeneralIssue>()
                                   .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet("/SsuIssue/page-general-unsolve")]
        public async Task<PageResult<OutputGeneralIssue>> PageByUnSolved([FromQuery] QueryListInUnSolve input)
        {
            var ssuIssuess = await this._ssuIssueRep.DetachedEntities
                                  .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                  .Where(input.Module != null, u => u.Module == input.Module)
                                  .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                  .Where(input.Status != null, u => (int)u.Status == input.Status)
                                  .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                  .OrderBy(PageInputOrder.OrderBuilder<QueryListInUnSolve>(input))
                                  .ProjectToType<OutputGeneralIssue>()
                                  .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }
    }
}
