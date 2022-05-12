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
using Newtonsoft.Json;
using QMS.Application.Issues.Field;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.IssueService.Dto.QueryList;
using QMS.Application.Issues.Service.SsuIssue.Attachment;
using QMS.Application.Issues.Service.SsuIssue.Dto;
using QMS.Application.Issues.Service.SsuIssue.Dto.Add;
using QMS.Application.Issues.Service.SsuIssue.Dto.Update;
using QMS.Core;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.Linq.Dynamic.Core;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题管理服务
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "Issue", Order = 100)]
    public class SsuIssueService : ISsuIssueService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssue, IssuesDbContextLocator> _ssuIssueRep;
        private readonly IRepository<SsuIssueDetail, IssuesDbContextLocator> _ssuIssueDetailRep;
        private readonly IRepository<SsuIssueOperation, IssuesDbContextLocator> _ssuIssueOperateRep;
        private readonly IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> _ssuIssueAttrRep;
        private readonly IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> _ssuIssueAttrValueRep;

        private readonly IHttpContextAccessor _contextAccessor;

        public SsuIssueService(
            IRepository<SsuIssue, IssuesDbContextLocator> ssuIssueRep,
            IRepository<SsuIssueDetail, IssuesDbContextLocator> ssuIssueDetailRep,
            IRepository<SsuIssueOperation, IssuesDbContextLocator> ssuIssueOperateRep,
            IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> ssuIssueAttrRep,
            IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> ssuIssueAttrValueRep,
            IHttpContextAccessor contextAccessor
        )
        {
            this._ssuIssueRep = ssuIssueRep;
            this._ssuIssueDetailRep = ssuIssueDetailRep;
            this._ssuIssueOperateRep = ssuIssueOperateRep;
            this._ssuIssueAttrRep = ssuIssueAttrRep;
            this._ssuIssueAttrValueRep = ssuIssueAttrValueRep;

            this._contextAccessor = contextAccessor;
        }

        #region CRUD
        private async Task AddAttributeValuesBatch(string ExtendAttribute)
        {
            if (!string.IsNullOrEmpty(ExtendAttribute))
            {
                List<FieldValue> list = JsonConvert.DeserializeObject<List<FieldValue>>(ExtendAttribute);

                await this._ssuIssueAttrValueRep.Entities.AddRangeAsync(list.Select<FieldValue, SsuIssueExtendAttributeValue>(model => new SsuIssueExtendAttributeValue
                {
                    Id = model.AttributeId,
                    IssueNum = model.IssueId,
                    AttibuteValue = model.Value
                }));

                await this._ssuIssueAttrValueRep.Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 增加问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/add")]
        public async Task<long> Add(InIssue input)
        {
            var ssuIssue = input.Adapt<SsuIssue>();
            ssuIssue.SetCreate();

            EntityEntry<SsuIssue> issue = await this._ssuIssueRep.InsertNowAsync(ssuIssue, ignoreNullValues: true);

            var detail = input.Adapt<SsuIssueDetail>();
            detail.Id = issue.Entity.Id;

            await this._ssuIssueDetailRep.InsertNowAsync(detail, ignoreNullValues: true);

            // 插入扩展字段数据
            await this.AddAttributeValuesBatch(input.ExtendAttribute);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                detail.Id,
                EnumIssueOperationType.New,
                "新建问题"
            );

            return detail.Id;
        }



        /// <summary>
        /// 删除问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/delete")]
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
        [HttpPost("/issue/edit")]
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
        [HttpGet("/issue/detail")]
        public async Task<OutputDetailIssue> Get([FromQuery] BaseId input)
        {
            return (await this._ssuIssueDetailRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<OutputDetailIssue>();
        }
        #endregion

        //[HttpGet("/SsuIssue/list")]
        //public async Task<List<SsuIssueOutput>> List([FromQuery] SsuIssueInput input)
        //{
        //    return await _ssuIssueRep.DetachedEntities.ProjectToType<SsuIssueOutput>().ToListAsync();
        //}

        #region 流程管理
        /// <summary>
        /// 执行者处理问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/execute")]
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
        [HttpPost("/issue/validate")]
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


            EnumIssueOperationType enumIssueOperationType = pass ? EnumIssueOperationType.Close : EnumIssueOperationType.NoPass;
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
        [HttpPost("/issue/hangup")]
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
        /// 重分派
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/redispatch")]
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
                EnumIssueOperationType.ReDispatch,
                $"【{common.Dispatcher.GetNameByEmpId()}】将【{common.CreatorId.GetNameByEmpId()}】提出的问题重分发给【{common.Executor.GetNameByEmpId()}】"
            );
        }
        #endregion

        #region 分页查询及导出
        private IQueryable<SsuIssue> GetQueryable(BaseQueryModel input)
        {
            IQueryable<SsuIssue> querable = this._ssuIssueRep.DetachedEntities
                                     .Where(input.ProjectId > 0, u => u.ProjectId == input.ProjectId)
                                     .Where(input.Module != null, u => u.Module == input.Module)
                                     .Where(input.Consequence != null, u => u.Consequence == input.Consequence)
                                     .Where(input.Status != null, u => u.Status == input.Status)
                                     .Where(!string.IsNullOrEmpty(input.Title), u => u.Title.Contains(input.Title))

                                     .Where(input.Creator != null, u => u.CreatorId == input.Creator)
                                     .Where(input.Dispatcher != null, u => u.Dispatcher == input.Dispatcher)
                                     .Where(input.Executor != null, u => u.Executor == input.Executor)

                                     .Where(input.CreateTimeFrom != null, u => u.CreateTime >= input.CreateTimeFrom)
                                     .Where(input.CreateTimeTo != null, u => u.CreateTime <= input.CreateTimeTo)
                                     .Where(input.DispatchTimeFrom != null, u => u.DispatchTime >= input.DispatchTimeFrom)
                                     .Where(input.DispatchTimeTo != null, u => u.DispatchTime <= input.DispatchTimeTo)
                                     .Where(input.SolveTimeFrom != null, u => u.SolveTime >= input.SolveTimeFrom)
                                     .Where(input.SolveTimeTo != null, u => u.SolveTime <= input.SolveTimeTo);

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
                    querable = querable.Where(item => item.Status == EnumIssueStatus.Solved);
                    break;
                case EnumQueryCondition.Unsolve:
                    querable = querable.Where(item => item.Status == EnumIssueStatus.UnSolve);
                    break;
                case EnumQueryCondition.Closed:
                    querable = querable.Where(item => item.Status == EnumIssueStatus.Closed);
                    break;
                case EnumQueryCondition.Hangup:
                    querable = querable.Where(item => item.Status == EnumIssueStatus.HasHangUp);
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
        [HttpGet("/issue/page")]
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
                    querable = querable.Where(item => item.IssueStatus == EnumIssueStatus.Solved);
                    break;
                case EnumQueryCondition.Unsolve:
                    querable = querable.Where(item => item.IssueStatus == EnumIssueStatus.UnSolve);
                    break;
                case EnumQueryCondition.Closed:
                    querable = querable.Where(item => item.IssueStatus == EnumIssueStatus.Closed);
                    break;
                case EnumQueryCondition.Hangup:
                    querable = querable.Where(item => item.IssueStatus == EnumIssueStatus.HasHangUp);
                    break;
            }
        }

        /// <summary>
        /// 问题数据导出到Excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/export")]
        public async Task<IActionResult> Export(BaseQueryModel input)
        {
            IQueryable<ExportIssueDto> querable = this.GetExportQuerable(input);

            this.AddFilter(input, querable);

            PageResult<ExportIssueDto> list = await querable.ToADPagedListAsync(input.PageNo, input.PageSize);

            Helper.Helper.Assert(list != null && list.Rows.Count > 0, "不存在任何问题记录!");

            return await Helper.Helper.ExportExcel(list.Rows);
        }
        #endregion

        #region 问题数据导入
        /// <summary>
        /// 下载Excel以方便导入问题数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("/issue/template")]
        public async Task<IActionResult> Template()
        {
            var item = this._ssuIssueRep.Where(model => model.Title != null).Take<SsuIssue>(1).ProjectToType<InIssue>();
            return await Helper.Helper.ExportExcel(item, "IssueTemplate");
        }

        /// <summary>
        /// 问题数据导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/issue/import")]
        public async Task ImportIssues(IFormFile file)
        {
            Helper.Helper.Assert(file != null && !string.IsNullOrEmpty(file.FileName), "文件为空");

            Helper.Helper.Assert(file.FileName, fileName => fileName.Contains("IssueTemplate") && fileName.EndsWith(".xlsx"), "请使用下载的模板进行数据导入");

            IEnumerable<dynamic> collection = MiniExcel.Query(file.OpenReadStream(), true);

            foreach (var item in collection)
            {
                var issue = new InIssue()
                {
                    Title = item.标题,
                    Description = item.详情,
                    ProjectId = Convert.ToInt64(item.项目编号),
                    ProductId = Convert.ToInt64(item.产品编号),
                    Module = (EnumModule)Helper.Helper.GetIntFromEnumDescription(item.问题模块),
                    Consequence = (EnumConsequence)Helper.Helper.GetIntFromEnumDescription(item.问题性质),
                    IssueClassification = (EnumIssueClassification)Helper.Helper.GetIntFromEnumDescription(item.问题分类),
                    Dispatcher = Convert.ToInt64(item.分发人编号),
                    Source = (EnumIssueSource)Helper.Helper.GetIntFromEnumDescription(item.问题来源),
                    Discover = Convert.ToInt64(item.发现人编号),
                    DiscoverTime = Convert.ToDateTime(item.发现日期),
                    Status = (EnumIssueStatus)Helper.Helper.GetIntFromEnumDescription(item.问题状态),
                    CreatorId = Convert.ToInt64(item.提出人编号),
                    CreateTime = Convert.ToDateTime(item.提出日期),
                    CC = Convert.ToInt64(item.被抄送人编号)
                };

                await this.Add(issue);
            }
        }
        #endregion

        #region 问题相关附件的信息保存和获取
        /// <summary>
        /// 附件上传后通知问题表保存附件id
        /// 20220511 逻辑改动：由原来的后端上传并保存Id信息改为前端上传、后端保存Id
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/issue/attachment/saveId")]
        public async Task SaveAttachment(AttachmentModel attachment, long issueId)
        {

            Helper.Helper.Assert(attachment != null && issueId != 0, "附件信息或问题编号为空!无法保存");

            SsuIssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, issueId);

            var list = new List<AttachmentModel>();
            if (!string.IsNullOrEmpty(detail.Attachments))
            {
                list = JsonConvert.DeserializeObject<List<AttachmentModel>>(detail.Attachments);
            }

            list.Add(new AttachmentModel()
            {
                //IssueId = issueId,
                AttachmentId = attachment.AttachmentId,
                FileName = attachment.FileName,
                AttachmentType = attachment.AttachmentType
            });

            detail.Attachments = JsonConvert.SerializeObject(list);

            await this._ssuIssueDetailRep.UpdateIncludeAsync(detail, new string[] { nameof(detail.Attachments) }, true);

            await Helper.IssueLogger.Log(this._ssuIssueOperateRep, issueId, EnumIssueOperationType.Upload, attachment.FileName);
        }

        /// <summary>
        /// 附件下载时获取该问题所属的附件编号等信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/attachment/infoList")]
        public async Task<List<AttachmentModel>> AttachmentInfoList(long issueId)
        {
            Helper.Helper.Assert(issueId != 0, "问题编号不合法,无法获取相关附件信息");

            SsuIssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, issueId);

            List<AttachmentModel> list = null;
            if (!string.IsNullOrEmpty(detail.Attachments))
            {
                list = JsonConvert.DeserializeObject<List<AttachmentModel>>(detail.Attachments);
            }

            return list;
        }
        #endregion

        #region 分发操作
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

            public string ExtendAttribute { get; set; }

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

        /// <summary>
        /// 分派
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/dispatch")]
        public async Task Dispatch(InDispatch input)
        {
            SsuIssue issue = await Helper.Helper.CheckIssueExist(this._ssuIssueRep, input.Id);
            issue.DoDispatch();
            input.SetIssue(issue);
            await this._ssuIssueRep.UpdateNowAsync(issue);


            SsuIssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._ssuIssueDetailRep, input.Id);
            await this._ssuIssueDetailRep.UpdateNowAsync(detail, null);

            await this.UpdateAttributeValuesBatch(input.ExtendAttribute);

            await IssueLogger.Log(
                this._ssuIssueOperateRep,
                input.Id,
                EnumIssueOperationType.Dispatch,
                $"【{issue.Dispatcher.GetNameByEmpId()}】将【{issue.CreatorId.GetNameByEmpId()}】提出的问题分发给【{issue.Executor.GetNameByEmpId()}】"
            );
        }

        private async Task UpdateAttributeValuesBatch(string ExtendAttribute)
        {
            if (!string.IsNullOrEmpty(ExtendAttribute))
            {
                List<FieldValue> list = JsonConvert.DeserializeObject<List<FieldValue>>(ExtendAttribute);

                this._ssuIssueAttrValueRep.Entities.UpdateRange(list.Select<FieldValue, SsuIssueExtendAttributeValue>(model => new SsuIssueExtendAttributeValue
                {
                    Id = model.AttributeId,
                    IssueNum = model.IssueId,
                    AttibuteValue = model.Value
                }));

                await this._ssuIssueAttrValueRep.Context.SaveChangesAsync();
            }
        }
        #endregion

        #region 问题列表列名管理
        ///// <summary>
        ///// 获取问题列表列名
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpGet("/issue/column/display")]
        //public async Task<List<KeyValuePair<string, string>>> GetColumnDisplay()
        //{
        //    return Helper.Helper.GetColumns();
        //}
        #endregion
    }
}
