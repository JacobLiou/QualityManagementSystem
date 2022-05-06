using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Furion.JsonSerialization;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiniExcelLibs;
using QMS.Application.Issues.Field;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.IssueService.Dto.QueryList;
using QMS.Application.Issues.Service.SsuIssue;
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
        private readonly IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> _ssuIssueAttrRep;
        private readonly IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> _ssuIssueAttrValueRep;

        public SsuIssueService(
            IRepository<SsuIssue, IssuesDbContextLocator> ssuIssueRep,
            IRepository<SsuIssueDetail, IssuesDbContextLocator> ssuIssueDetailRep,
            IRepository<SsuIssueOperation, IssuesDbContextLocator> ssuIssueOperateRep,
            IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> ssuIssueAttrRep,
            IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> ssuIssueAttrValueRep
        )
        {
            this._ssuIssueRep = ssuIssueRep;
            this._ssuIssueDetailRep = ssuIssueDetailRep;
            this._ssuIssueOperateRep = ssuIssueOperateRep;
            this._ssuIssueAttrRep = ssuIssueAttrRep;
            this._ssuIssueAttrValueRep = ssuIssueAttrValueRep;
        }

        /// <summary>
        /// 增加问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/Add")]
        public async Task Add(InIssue input)
        {
            var ssuIssue = input.Adapt<SsuIssue>();
            ssuIssue.SetCreate();

            EntityEntry<SsuIssue> issue = await this._ssuIssueRep.InsertNowAsync(ssuIssue, ignoreNullValues: true);

            // 插入扩展字段数据
            await this.AddAttributeValuesBatch(input.ExtendAttribute);

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

        private async Task AddAttributeValuesBatch(string ExtendAttribute)
        {
            if (!string.IsNullOrEmpty(ExtendAttribute))
            {
                List<FieldValue> list = App.GetService<IJsonSerializerProvider>().Deserialize<List<FieldValue>>(ExtendAttribute);


                await this._ssuIssueAttrValueRep.Entities.AddRangeAsync(list.Select<FieldValue, SsuIssueExtendAttributeValue>(model => new SsuIssueExtendAttributeValue
                {
                    Id = model.AttributeId,
                    IssueNum = model.IssueId,
                    AttibuteValue = model.Value
                }));

                await this._ssuIssueAttrValueRep.Context.SaveChangesAsync();
            }
        }

        #region 已测试完毕
        /// <summary>
        /// 删除问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/Delete")]
        public async Task Delete(DeleteSsuIssueInput input)
        {
            SsuIssue ssuIssue = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            if (!ssuIssue.IsDeleted)
            {
                ssuIssue.IsDeleted = true;

                await this._ssuIssueRep.UpdateNowAsync(ssuIssue);

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
        [HttpPost("/SsuIssue/Edit")]
        public async Task Edit(UpdateSsuIssueInput input)
        {
            SsuIssue ssuIssue = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            //var isExist = await _ssuIssueRep.AnyAsync(u => u.Id == input.Id, false);
            //if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            //ssuIssue = input.Adapt<SsuIssue>();

            // 手动更新，防止已有数据丢失
            input.SetIssue(ssuIssue);
            await _ssuIssueRep.UpdateNowAsync(ssuIssue, ignoreNullValues: true);

            SsuIssueDetail ssuIssueDetail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, input.Id);

            input.SetIssueDetail(ssuIssueDetail);
            await this._ssuIssueDetailRep.UpdateNowAsync(ssuIssueDetail, true);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.Edit,
                "更新问题"
            );
        }


        /// <summary>
        /// 获取问题详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssue/Detail")]
        public async Task<OutputDetailIssue> Get([FromQuery] BaseId input)
        {
            return (await this._ssuIssueDetailRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<OutputDetailIssue>();
        }

        //[HttpGet("/SsuIssue/list")]
        //public async Task<List<SsuIssueOutput>> List([FromQuery] SsuIssueInput input)
        //{
        //    return await _ssuIssueRep.DetachedEntities.ProjectToType<SsuIssueOutput>().ToListAsync();
        //}

        /// <summary>
        /// 执行者处理问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/Execute")]
        public async Task Execute(InSolve input)
        {
            SsuIssue common = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            common.DoSolve();
            input.SetIssue(common);
            await this._ssuIssueRep.UpdateNowAsync(common, true);

            SsuIssueDetail ssuIssueDetail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, input.Id);
            input.SetIssueDetail(ssuIssueDetail);
            await this._ssuIssueDetailRep.UpdateNowAsync(ssuIssueDetail, true);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.Solve,
                $"【{common.Executor.GetNameByEmpId()}】解决【{common.CreatorId.GetNameByEmpId()}】提出的问题"
            );
        }

        /// <summary>
        /// 提出者或验证者验证问题是否解决
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/Validate")]
        public async Task Validate(InValidate input)
        {
            SsuIssue common = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);
            bool pass = input.PassResult == YesOrNot.Y;
            common.DoVerify(pass);
            input.SetIssue(common);
            await this._ssuIssueRep.UpdateNowAsync(common, true);


            SsuIssueDetail ssuIssueDetail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, input.Id);
            input.SetIssueDetail(ssuIssueDetail);
            await this._ssuIssueDetailRep.UpdateNowAsync(ssuIssueDetail, true);


            EnumIssueOperationType enumIssueOperationType = pass ? EnumIssueOperationType.NoPass : EnumIssueOperationType.Close;
            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                enumIssueOperationType,
                $"验证【{common.Executor.GetNameByEmpId()}】处理的问题,结果是【" + (pass ? "通过" : "不通过") + "】"
                );
        }
        
        /// <summary>
        /// 挂起问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/Hangup")]
        public async Task HangUp(InHangup input)
        {
            SsuIssue common = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);

            common.SetHangup();
            input.SetIssue(common);
            await this._ssuIssueRep.UpdateNowAsync(common, true);

            SsuIssueDetail ssuIssueDetail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, input.Id);
            if (input.SetIssueDetail(ssuIssueDetail))
            {
                await this._ssuIssueDetailRep.UpdateNowAsync(ssuIssueDetail, true);
            }

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.HangUp,
                $"【{common.HangupId.GetNameByEmpId()}】挂起【{common.CreatorId.GetNameByEmpId()}】提出的问题"
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssue/Redispatch")]
        public async Task ReDispatch(InReDispatch input)
        {
            SsuIssue common = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);
            common.DoDispatch();
            input.SetIssue(common);

            await this._ssuIssueRep.UpdateNowAsync(common, true);

            SsuIssueDetail ssuIssueDetail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, input.Id);
            if (input.SetIssueDetail(ssuIssueDetail))
            {
                await this._ssuIssueDetailRep.UpdateNowAsync(ssuIssueDetail, true);
            }

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.Dispatch,
                $"【{common.Dispatcher.GetNameByEmpId()}】将【{common.CreatorId.GetNameByEmpId()}】提出的问题重分发给【{common.Executor.GetNameByEmpId()}】"
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
                case EnumQueryCondition.CC:
                    querable = querable.Where(item => item.CC == Helper.Helper.GetCurrentUser());
                    break;
            }

            return querable;
        }

        /// <summary>
        /// 多种条件分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssue/Page")]
        public async Task<PageResult<OutputGeneralIssue>> Page([FromQuery] BaseQueryModel input)
        {
            IQueryable<SsuIssue> querable = this.GetQueryable(input);

            var ssuIssues = await querable.OrderBy(PageInputOrder.OrderBuilder<BaseQueryModel>(input))
                                     .Select<SsuIssue, OutputGeneralIssue>(issue => new OutputGeneralIssue(issue))
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssues;
        }
        #endregion

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

        [HttpPost("/SsuIssue/Export")]
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
        [HttpPost("/SsuIssue/UploadFile")]
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


        public class InDispatch : IInput
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public DateTime? ForecastSolveTime { get; set; }
            public EnumConsequence Consequence { get; set; }
            public EnumIssueClassification IssueClassification { get; set; }
            /// <summary>
            /// 执行人
            /// </summary>
            public long? Executor { get; set; }
            public long? CC { get; set; }

            public string JsonModel { get; set; }

            public bool SetIssue(SsuIssue issue)
            {
                bool changed = false;

                if (issue.Title != this.Title)
                {
                    issue.Title = this.Title;

                    changed = true;
                }

                issue.IssueClassification = this.IssueClassification;
                issue.Consequence = this.Consequence;
                issue.Executor = this.Executor;
                issue.CC = this.CC;
                issue.ForecastSolveTime = this.ForecastSolveTime;

                return changed;
            }

            public bool SetIssueDetail(SsuIssueDetail issueDetail)
            {

                return true;
            }
        }

        [HttpPost("/SsuIssue/Dispatch")]
        public async Task Dispatch(InDispatch input)
        {
            SsuIssue issue = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);
            issue.DoDispatch();
            input.SetIssue(issue);
            await this._ssuIssueRep.UpdateNowAsync(issue);

            SsuIssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, input.Id);
            await this._ssuIssueDetailRep.UpdateNowAsync(detail);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.Dispatch,
                $"【{issue.Dispatcher.GetNameByEmpId()}】将【{issue.CreatorId.GetNameByEmpId()}】提出的问题分发给【{issue.Executor.GetNameByEmpId()}】"
            );
        }
    }
}
