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
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.IssueService.Dto.QueryList;
using QMS.Application.Issues.Service.SsuIssue.Dto;
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

        private readonly IRepository<SsuIssueOperation, IssuesDbContextLocator> _ssuIssueOperateRep;


        public SsuIssueService(
            IRepository<SsuIssue, IssuesDbContextLocator> ssuIssueRep,
            IRepository<SsuIssueDetail, IssuesDbContextLocator> ssuIssueDetailRep,
            IRepository<SsuIssueOperation, IssuesDbContextLocator> ssuIssueOperateRep
        )
        {
            this._ssuIssueRep = ssuIssueRep;
            this._ssuIssueDetailRep = ssuIssueDetailRep;
            this._ssuIssueOperateRep = ssuIssueOperateRep;

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
            ssuIssue.Status = Core.Enum.EnumIssueStatus.Created;

            EntityEntry<SsuIssue> issue = await this._ssuIssueRep.InsertNowAsync(ssuIssue, ignoreNullValues: true).ConfigureAwait(false);

            var detail = input.Adapt<SsuIssueDetail>();
            detail.Id = issue.Entity.Id;

            await this._ssuIssueDetailRep.InsertNowAsync(detail, ignoreNullValues: true);

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                IssueId = detail.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.New,
                OperationTime = ssuIssue.CreateTime,
                OperatorName = ssuIssue.CreatorId.GetNameByEmpId(),
                Content = "新建问题"
            }, true);
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

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.Close,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = "删除问题"
            }, true);
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

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.Edit,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = "更新问题"
            }, true);
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
            common.Status = Core.Enum.EnumIssueStatus.Solved;
            common.SolveTime = input.SolveTime ?? DateTime.Now;
            common.Executor = Helper.Helper.GetCurrentUser();

            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.Solve,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = "处理问题"
            }, true);
        }

        [HttpPost("/SsuIssue/validate")]
        public async Task Update(InValidate input)
        {
            bool pass = input.PassResult == YesOrNot.Y;
            var common = input.Adapt<SsuIssue>();
            common.Status = pass ? Core.Enum.EnumIssueStatus.Closed : Core.Enum.EnumIssueStatus.UnSolve;
            common.ValidateTime = input.ValidateTime ?? DateTime.Now;
            common.Verifier = Helper.Helper.GetCurrentUser();

            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);

            SsuIssueOperation ssuIssueOperation = new SsuIssueOperation()
            {
                IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.NoPass,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = $"验证【{common.Executor.GetNameByEmpId()}】处理的问题"
            };

            if (pass)
            {
                ssuIssueOperation.OperationTypeId = Core.Enum.EnumIssueOperationType.Close;
            }

            await this._ssuIssueOperateRep.InsertAsync(ssuIssueOperation, true);
        }

        [HttpPost("/SsuIssue/hangup")]
        public async Task Update(InHangup input)
        {
            var common = input.Adapt<SsuIssue>();
            common.Status = Core.Enum.EnumIssueStatus.HasHangUp;

            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.HangUp,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = "挂起问题"
            }, true);
        }

        [HttpPost("/SsuIssue/redispatch")]
        public async Task Update(InReDispatch input)
        {
            var common = input.Adapt<SsuIssue>();
            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.Dispatch,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = "重分发问题"
            }, true);
        }

        private IQueryable<SsuIssue> GetQueryable(BaseQueryModel input)
        {
            IQueryable<SsuIssue> querable = this._ssuIssueRep.DetachedEntities
                                     .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                     .Where(input.Module != null, u => u.Module == input.Module)
                                     .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                     .Where(input.Status != null, u => u.Status == input.Status)
                                     .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord));

            switch (input.QueryCondition)
            {
                case EnumQueryCondition.Creator:
                    querable = querable.Where(item => item.CreatorId == Helper.Helper.GetCurrentUser());
                    break;
                case EnumQueryCondition.Dispatcher:
                    querable = querable.Where(item => item.Dispatcher == Helper.Helper.GetCurrentUser());
                    break;
                case EnumQueryCondition.Executor:
                    querable = querable.Where(item => item.Executor == Helper.Helper.GetCurrentUser());
                    break;
                case EnumQueryCondition.Solved:
                    querable = querable.Where(item => item.Status == Core.Enum.EnumIssueStatus.Solved);
                    break;
                case EnumQueryCondition.Unsolve:
                    querable = querable.Where(item => item.Status == Core.Enum.EnumIssueStatus.UnSolve);
                    break;
            }

            return querable;
        }

        /// <summary>
        /// 多种条件分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssue/page")]
        public async Task<PageResult<OutputGeneralIssue>> Page([FromQuery] BaseQueryModel input)
        {
            IQueryable<SsuIssue> querable = this.GetQueryable(input);

            var ssuIssues = await querable.OrderBy(PageInputOrder.OrderBuilder<BaseQueryModel>(input))
                                     .Select<SsuIssue, OutputGeneralIssue>(issue => new OutputGeneralIssue(issue))
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssues;
        }

        private IQueryable<ExportIssueDto> GetExportQuerable(BaseQueryModel input)
        {
            return this.GetQueryable(input).Join<SsuIssue, SsuIssueDetail, long, ExportIssueDto>(
                                        this._ssuIssueDetailRep.DetachedEntities,
                                        issue => issue.Id,
                                        detailIssue => detailIssue.Id,
                                        (issue, detail) => new ExportIssueDto(issue, detail)
                                      );
        }

        private void AddFilter(BaseQueryModel input, IQueryable<ExportIssueDto> querable)
        {
            switch (input.QueryCondition)
            {
                case EnumQueryCondition.Creator:
                    querable = querable.Where(item => item.CreatorId == Helper.Helper.GetCurrentUser());
                    break;
                case EnumQueryCondition.Dispatcher:
                    querable = querable.Where(item => item.DispatcherId == Helper.Helper.GetCurrentUser());
                    break;
                case EnumQueryCondition.Executor:
                    querable = querable.Where(item => item.ExecutorId == Helper.Helper.GetCurrentUser());
                    break;
                case EnumQueryCondition.Solved:
                    querable = querable.Where(item => item.IssueStatus == Core.Enum.EnumIssueStatus.Solved);
                    break;
                case EnumQueryCondition.Unsolve:
                    querable = querable.Where(item => item.IssueStatus == Core.Enum.EnumIssueStatus.UnSolve);
                    break;
            }
        }

        [HttpGet("/SsuIssue/export")]
        public async Task Export([FromQuery] BaseQueryModel input)
        {
            IQueryable<ExportIssueDto> querable = this.GetExportQuerable(input);

            this.AddFilter(input, querable);

            PageResult<ExportIssueDto> list = await querable.ToADPagedListAsync(input.PageNo, input.PageSize); ;

            CsvFileHelper.SaveCsv<ExportIssueDto>(list.Rows, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Data/" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".csv");
        }
    }
}
