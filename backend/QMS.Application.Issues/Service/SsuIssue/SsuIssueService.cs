using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiniExcelLibs;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.IssueService.Dto.QueryList;
using QMS.Application.Issues.Service.SsuIssue.Dto;
using QMS.Application.Issues.Service.SsuIssue.Dto.Add;
using QMS.Application.Issues.Service.SsuIssue.Dto.Update;
using QMS.Core;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.Linq.Dynamic.Core;
using Yitter.IdGenerator;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题管理服务
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "SsuIssue", Order = 100)]
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
            ssuIssue.SetCreate();

            EntityEntry<SsuIssue> issue = await this._ssuIssueRep.InsertNowAsync(ssuIssue, ignoreNullValues: true);

            var detail = input.Adapt<SsuIssueDetail>();
            detail.Id = issue.Entity.Id;

            await this._ssuIssueDetailRep.InsertNowAsync(detail, ignoreNullValues: true);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                detail.Id,
                EnumIssueOperationType.New,
                "新建问题"
            );
        }

        /// <summary>
        /// 删除问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/delete")]
        public async Task Delete(DeleteSsuIssueInput input)
        {
            SsuIssue ssuIssue = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            if (!ssuIssue.IsDeleted)
            {
                ssuIssue.IsDeleted = true;

                await this._ssuIssueRep.UpdateAsync(ssuIssue);

                await IssueLogger.Log(
                    this._ssuIssueOperateRep,
                    input.Id,
                    EnumIssueOperationType.Close,
                    "删除问题"
                );
            }
            //else
            //{
            //    await this._ssuIssueRep.DeleteAsync(ssuIssue);
            //}
        }

        /// <summary>
        /// 更新问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/edit")]
        public async Task Edit(UpdateSsuIssueInput input)
        {
            SsuIssue ssuIssue = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            //var isExist = await _ssuIssueRep.AnyAsync(u => u.Id == input.Id, false);
            //if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            ssuIssue = input.Adapt<SsuIssue>();
            await _ssuIssueRep.UpdateAsync(ssuIssue, ignoreNullValues: true);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.Edit,
                "更新问题"
            );
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
        public async Task Execute(InSolve input)
        {
            SsuIssue common = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            common = input.Adapt<SsuIssue>();
            common.SetSolve();

            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.Solve,
                "处理问题"
            );
        }

        [HttpPost("/SsuIssue/validate")]
        public async Task Validate(InValidate input)
        {
            SsuIssue common = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            bool pass = input.PassResult == YesOrNot.Y;
            common = input.Adapt<SsuIssue>();

            common.SetVerify(pass);

            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);

            EnumIssueOperationType enumIssueOperationType = pass ? EnumIssueOperationType.NoPass : EnumIssueOperationType.Close;

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                enumIssueOperationType,
                $"验证【{common.Executor.GetNameByEmpId()}】处理的问题,结果是【" + (pass ? "通过" : "不通过" + "】")
                );
        }

        [HttpPost("/SsuIssue/hangup")]
        public async Task HangUp(InHangup input)
        {
            SsuIssue common = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            common = input.Adapt<SsuIssue>();

            common.SetHangup();

            var detail = input.Adapt<SsuIssueDetail>();

            await this._ssuIssueRep.UpdateAsync(common, true);
            await this._ssuIssueDetailRep.UpdateAsync(detail, true);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.HangUp,
                "挂起问题"
            );
        }

        [HttpPost("/SsuIssue/redispatch")]
        public async Task ReDispatch(InReDispatch input)
        {
            SsuIssue common = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            common = input.Adapt<SsuIssue>();

            common.SetDispatch();

            await this._ssuIssueRep.UpdateAsync(common, true);

            if (!string.IsNullOrEmpty(input.Comment))
            {
                var detail = input.Adapt<SsuIssueDetail>();
                await this._ssuIssueDetailRep.UpdateAsync(detail, true);
            }

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.Dispatch,
                "重分发问题"
            );
        }

        private IQueryable<SsuIssue> GetQueryable(BaseQueryModel input)
        {
            IQueryable<SsuIssue> querable = this._ssuIssueRep.DetachedEntities
                                     .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                     .Where(input.Module != null, u => u.Module == input.Module)
                                     .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                     .Where(input.Status != null, u => u.Status == input.Status)
                                     .Where(!string.IsNullOrEmpty(input.Title), u => u.Title.Contains(input.Title));

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
                case EnumQueryCondition.Closed:
                    querable = querable.Where(item => item.Status == Core.Enum.EnumIssueStatus.Closed);
                    break;
                case EnumQueryCondition.Hangup:
                    querable = querable.Where(item => item.Status == Core.Enum.EnumIssueStatus.HasHangUp);
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
                case EnumQueryCondition.Closed:
                    querable = querable.Where(item => item.IssueStatus == Core.Enum.EnumIssueStatus.Closed);
                    break;
                case EnumQueryCondition.Hangup:
                    querable = querable.Where(item => item.IssueStatus == Core.Enum.EnumIssueStatus.HasHangUp);
                    break;
            }
        }

        [HttpPost("/SsuIssue/export")]
        public async Task<IActionResult> Export(BaseQueryModel input)
        {
            IQueryable<ExportIssueDto> querable = this.GetExportQuerable(input);

            this.AddFilter(input, querable);

            PageResult<ExportIssueDto> list = await querable.ToADPagedListAsync(input.PageNo, input.PageSize);

            var memoryStream = new MemoryStream();
            memoryStream.SaveAs(list.Rows);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return await Task.FromResult(new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = DateTime.Now.ToString("yyyyMMddHHmmss") + "partial-issues.xlsx"
            });
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/upload-file")]
        public async Task AttachmentUpload(IFormFile file)
        {
            var path = Path.Combine(Path.GetTempPath(), $"{YitIdHelper.NextId()}-{file.FileName}");
            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            //var rows = MiniExcel.Query(path); // 解析
            //foreach (var row in rows)
            //{
            //    var a = row.A;
            //    var b = row.B;
            //    // 入库等操作

            //}
        }

        /// <summary>
        /// 批量导入属性数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/import-attribute")]
        public async Task ImportAttributeData(IFormFile file)
        {
            var path = Path.Combine(Path.GetTempPath(), $"{YitIdHelper.NextId()}-{file.FileName}");
            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            //var rows = MiniExcel.Query(path); // 解析
            //foreach (var row in rows)
            //{
            //    var a = row.A;
            //    var b = row.B;
            //    // 入库等操作

            //}
        }


        public class InDispatch
        {
            public long Id { get; set; }
            public string JsonModel { get; set; }
        }

        [HttpPost("/SsuIssue/dispatch")]
        public async Task Dispatch(InDispatch input)
        {
            SsuIssue issue = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);
            issue.SetDispatch();

            SsuIssueDetail detail = await this._ssuIssueDetailRep.FirstOrDefaultAsync(detail => detail.Id == input.Id);

            this._ssuIssueRep.UpdateAsync(issue);
            this._ssuIssueDetailRep.UpdateAsync(detail);
        }
    }
}
