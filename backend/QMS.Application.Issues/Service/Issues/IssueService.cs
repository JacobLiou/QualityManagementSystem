using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiniExcelLibs;
using Newtonsoft.Json;
using QMS.Application.Issues.Field;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.Service.Issue.Attachment;
using QMS.Application.Issues.Service.Issue.Dto;
using QMS.Application.Issues.Service.Issue.Dto.Add;
using QMS.Application.Issues.Service.Issue.Dto.Query;
using QMS.Application.Issues.Service.Issue.Dto.Update;
using QMS.Application.Issues.Service.Issues.AnalyzeData;
using QMS.Application.Issues.Service.Issues.Dto.Update;
using QMS.Application.Issues.Service.ThirdPartyService.Dto;
using QMS.Core;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.Linq.Dynamic.Core;
using static QMS.Application.Issues.IssueStatusNoticeService;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题管理服务
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "Issue", Order = 100)]
    [Route("issue")]
    public partial class IssueService : IIssueService, IDynamicApiController, ITransient
    {
        private readonly IRepository<Issue, IssuesDbContextLocator> _issueRep;
        private readonly IRepository<IssueDetail, IssuesDbContextLocator> _issueDetailRep;
        private readonly IRepository<IssueOperation, IssuesDbContextLocator> _issueOperateRep;
        private readonly IRepository<IssueExtendAttribute, IssuesDbContextLocator> _issueAttrRep;
        private readonly IRepository<IssueExtendAttributeValue, IssuesDbContextLocator> _issueAttrValueRep;
        private readonly IRepository<IssueColumnDisplay, IssuesDbContextLocator> _issueColumnDisplayRep;

        private readonly IssueStatusNoticeService _noticeService;
        private readonly IssueCacheService _issueCacheService;

        public IssueService(
            IRepository<Issue, IssuesDbContextLocator> issueRep,
            IRepository<IssueDetail, IssuesDbContextLocator> issueDetailRep,
            IRepository<IssueOperation, IssuesDbContextLocator> issueOperateRep,
            IRepository<IssueExtendAttribute, IssuesDbContextLocator> issueAttrRep,
            IRepository<IssueExtendAttributeValue, IssuesDbContextLocator> issueAttrValueRep,
            IRepository<IssueColumnDisplay, IssuesDbContextLocator> issueColumnDisplayRep,
            IssueStatusNoticeService noticeService,
            IssueCacheService issueCacheService

        )
        {
            this._issueRep = issueRep;
            this._issueDetailRep = issueDetailRep;
            this._issueOperateRep = issueOperateRep;
            this._issueAttrRep = issueAttrRep;
            this._issueAttrValueRep = issueAttrValueRep;
            this._issueColumnDisplayRep = issueColumnDisplayRep;
            this._noticeService = noticeService;
            this._issueCacheService = issueCacheService;
        }

        #region CRUD

        private async Task AddAttributeValuesBatch(string extendAttribute)
        {
            if (!string.IsNullOrEmpty(extendAttribute))
            {
                var list = JSON.Deserialize<List<FieldValue>>(extendAttribute);
                await this._issueAttrValueRep.Entities.AddRangeAsync(list.Select(model => new IssueExtendAttributeValue
                {
                    Id = model.FieldId,
                    IssueNum = model.IssueId,
                    AttibuteValue = model.Value
                }));

                await this._issueAttrValueRep.Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 增加问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<BaseId> Add(InIssue input)
        {
            Helper.Helper.CheckInput(input);

            var issue = input.Adapt<Issue>();
            if (input.CCList != null && input.CCList.Count > 0)
            {
                issue.CCs = JSON.Serialize(input.CCList);
            }
            //获取序号
            issue.SerialNumber = this.GetNewSerialNumber(issue.Module);

            Helper.Helper.Assert(issue.CurrentAssignment != null, "创建问题时必须要指定当前指派人!");
            input.SetIssuse(issue);

            EntityEntry<Issue> issueEntity = await this._issueRep.InsertNowAsync(issue, ignoreNullValues: true);

            var detail = input.Adapt<IssueDetail>();
            detail.Id = issueEntity.Entity.Id;

            if (!string.IsNullOrEmpty(detail.ExtendAttribute))
            {
                var list = JSON.Deserialize<List<FieldValue>>(detail.ExtendAttribute);

                foreach (var item in list)
                {
                    item.IssueId = detail.Id;
                }

                detail.ExtendAttribute = JSON.Serialize(list);
            }

            await this._issueDetailRep.InsertNowAsync(detail, ignoreNullValues: true);

            // 插入扩展字段数据
            await this.AddAttributeValuesBatch(detail.ExtendAttribute);

            //问题新增，如果当前用户和当前指派(分发人)不一致，则发送消息给对应的分发人
            if (!input.IsTemporary && Helper.Helper.GetCurrentUser() != input.CurrentAssignment)
            {
                _noticeService.SendNotice(issueEntity.Entity.Id.ToString(), input.CurrentAssignment.ToString(), issueEntity.Entity.Title);
            }
            //问题新增发送消息给抄送人
            if (input.CCList != null && input.CCList.Count > 0)
            {
                foreach (long id in input.CCList)
                {
                    _noticeService.SendNotice(id.ToString(), input.CurrentAssignment.ToString(), issueEntity.Entity.Title);
                }
            }

            await IssueLogger.Log(
                this._issueOperateRep,
                detail.Id,
                EnumIssueOperationType.New,
                "新建问题"
            );

            return new BaseId() { Id = detail.Id };
        }



        /// <summary>
        /// 删除问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task Delete(DeleteIssueInput input)
        {
            Helper.Helper.CheckInput(input);

            Issue issue = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            if (!issue.IsDeleted)
            {
                issue.IsDeleted = true;

                await this._issueRep.UpdateNowAsync(issue);

                await IssueLogger.Log(
                    this._issueOperateRep,
                    input.Id,
                    EnumIssueOperationType.Close,
                    "删除问题"
                );
            }
            //else
            //{
            //    await this._issueRep.DeleteAsync(issue);
            //}
        }

        /// <summary>
        /// 更新问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task Edit(UpdateIssueInput input)
        {
            Helper.Helper.CheckInput(input);

            Issue issue = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            //var isExist = await _issueRep.AnyAsync(u => u.Id == input.Id, false);
            //if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            //issue = input.Adapt<issue>();

            // 手动更新，防止已有数据丢失
            input.SetIssue(issue);
            await _issueRep.UpdateNowAsync(issue, true);

            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);

            if (input.SetIssueDetail(issueDetail))
            {
                await this._issueDetailRep.UpdateNowAsync(issueDetail);

                if (!string.IsNullOrEmpty(input.ExtendAttribute))
                {
                    // 新增扩展属性时
                    List<FieldValue> list = JSON.Deserialize<List<FieldValue>>(input.ExtendAttribute);

                    foreach (var item in list)
                    {
                        item.IssueId = input.Id;
                    }

                    var attrs = JSON.Serialize(list);

                    await this.UpdateAttributeValuesBatch(attrs);
                }
                else
                {
                    if (this._issueAttrValueRep.Any(val => val.IssueNum == input.Id))
                    {
                        await this._issueAttrValueRep.DeleteNowAsync(this._issueAttrValueRep.Where(val => val.IssueNum == input.Id));
                    }
                }
            }

            //如果当前指派人和当前用户不一致则发送消息给当前指派人
            if (input.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                _noticeService.SendNotice(issue.Id.ToString(), input.CurrentAssignment.ToString(), issue.Title);
            }

            await IssueLogger.Log(
                this._issueOperateRep,
                input.Id,
                EnumIssueOperationType.Edit,
                "更新问题" + JSON.Serialize(input)
            );
        }


        /// <summary>
        /// 获取问题详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("detail")]
        public async Task<OutputDetailIssue> Get([FromQuery] BaseId input)
        {
            Helper.Helper.CheckInput(input);

            Issue issue = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            IssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);

            OutputDetailIssue outputDetailIssue = detail.Adapt<OutputDetailIssue>();


            outputDetailIssue.SetCommon(issue);
            outputDetailIssue.BtnList = this.GetBtnList(issue);

            if (!string.IsNullOrEmpty(outputDetailIssue.ExtendAttribute))
            {
                List<FieldValue> list = JSON.Deserialize<List<FieldValue>>(outputDetailIssue.ExtendAttribute);

                var attrColl = this._issueAttrRep.DetachedEntities.Where<IssueExtendAttribute>(model => model.Module == list[0].Module);
                var attrIds = attrColl.Select<IssueExtendAttribute, long>(attr => attr.Id);

                List<FieldValue> fieldValues = list.Where(val => attrIds.Contains(val.FieldId)).ToList();
                var fieldIds = fieldValues.Select<FieldValue, long>(val => val.FieldId);

                foreach (var item in attrColl)
                {
                    if (!fieldIds.Contains(item.Id))
                    {
                        fieldValues.Add(new FieldValue()
                        {
                            IssueId = input.Id,
                            FieldCode = item.AttributeCode,
                            FieldName = item.AttibuteName,
                            FieldDataType = item.ValueType,
                            FieldId = item.Id,
                            Module = item.Module,
                            Value = string.Empty
                        });
                    }
                }

                outputDetailIssue.ExtendAttribute = JSON.Serialize(fieldValues);
            }

            return outputDetailIssue;
        }

        #endregion CRUD

        #region 流程管理

        /// <summary>
        /// 挂起后重新开启问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("reopen")]
        public async Task ReOpen(BaseId input)
        {
            Helper.Helper.CheckInput(input);

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            //已挂起，已关闭状态下支持重开启
            //Helper.Helper.Assert(common.Status == EnumIssueStatus.HasHangUp || common.Status == EnumIssueStatus.Closed, "必须为已挂起或者已关闭状态才能开启");
            //Helper.Helper.Assert(common.CurrentAssignment == Helper.Helper.GetCurrentUser(), "必须为问题分发人才能重开启问题");

            common.Status = EnumIssueStatus.Created;
            await this._issueRep.UpdateNowAsync(common, true);

            //更改逻辑为：必须为问题分发人才能重开启问题,重开启后下一步操作为问题分发人，所以此处不发送消息
            //如果当前用户和问题的分发人不一致，则重新开启后发送消息通知分发人
            //if (Helper.Helper.GetCurrentUser() != common.Dispatcher)
            //{
            //    this.SendNotice(common.Id.ToString(), common.Dispatcher.ToString(), common.Title);
            //}

            await IssueLogger.Log(
                this._issueOperateRep,
                input.Id,
                EnumIssueOperationType.ReOpen,
                $"【{Helper.Helper.GetCurrentUser().GetNameByEmpId()}】重开启已挂起的问题"
            );
        }

        /// <summary>
        /// 分发人复核问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("recheck")]
        public async Task ReCheck(InReCheck input)
        {
            Helper.Helper.CheckInput(input);

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            //Helper.Helper.Assert(common.CurrentAssignment != null && common.CurrentAssignment == Helper.Helper.GetCurrentUser(), $"当前指派人不是当前用户(复核人)");

            bool pass = input.PassResult == YesOrNot.Y;
            input.SetIssue(common);
            await this._issueRep.UpdateNowAsync(common, true);

            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            if (input.SetIssueDetail(issueDetail))
            {
                await this._issueDetailRep.UpdateNowAsync(issueDetail, true);
            }

            //问题复核有效，则问题流转至验证，如果当前指派（验证人）和当前用户不一致则发送消息给验证人
            if (pass && common.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                _noticeService.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
            }
            //问题复核无效，则问题重新流转至解决，如果当前指派（解决人）和当前用户不一致则发送消息给解决人
            if (!pass && common.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                _noticeService.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
            }

            await IssueLogger.Log(
                this._issueOperateRep,
                input.Id,
                EnumIssueOperationType.ReCheck,
                $"复核【{common.Executor.GetNameByEmpId()}】处理的问题,结果是【" + (pass ? "通过" : "不通过") + "】"
                );
        }

        /// <summary>
        /// 执行者处理问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task Execute(InSolve input)
        {
            Helper.Helper.CheckInput(input);

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            //Helper.Helper.Assert(common.CurrentAssignment != null && common.CurrentAssignment == Helper.Helper.GetCurrentUser(), $"当前指派人不是当前用户(执行人)");

            input.SetIssue(common);
            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            input.SetIssueDetail(issueDetail);

            await this._issueRep.UpdateNowAsync(common, true);

            await this._issueDetailRep.UpdateNowAsync(issueDetail, true);

            //如果当前用户和复核人（分发人）不一致，则发送消息给复核人（分发人）
            if (common.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                _noticeService.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
            }

            await IssueLogger.Log(
                this._issueOperateRep,
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
        [HttpPost("validate")]
        public async Task Validate(InValidate input)
        {
            Helper.Helper.CheckInput(input);

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            Helper.Helper.Assert(common.CurrentAssignment != null && common.CurrentAssignment == Helper.Helper.GetCurrentUser(), $"当前指派人不是当前用户(验证人)");

            bool pass = input.PassResult == YesOrNot.Y;
            input.SetIssue(common);
            await this._issueRep.UpdateNowAsync(common, true);


            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            input.SetIssueDetail(issueDetail);
            await this._issueDetailRep.UpdateNowAsync(issueDetail, true);

            //问题验证有效，则问题变更成关闭状态不需要发送消息
            //问题验证无效，则问题重新流转至分发状态，当前指派（分发人）和当前用户不一致则发送消息分发人
            if (!pass && common.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                _noticeService.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
            }

            EnumIssueOperationType enumIssueOperationType = input.PassResult == YesOrNot.Y ? EnumIssueOperationType.Close : EnumIssueOperationType.NoPass;
            await IssueLogger.Log(
                this._issueOperateRep,
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
        [HttpPost("hangup")]
        public async Task HangUp(InHangup input)
        {
            Helper.Helper.CheckInput(input);

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            input.SetIssue(common);

            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            if (input.SetIssueDetail(issueDetail))
            {
                await this._issueDetailRep.UpdateNowAsync(issueDetail, true);
            }

            await this._issueRep.UpdateNowAsync(common, true);

            await IssueLogger.Log(
                this._issueOperateRep,
                input.Id,
                EnumIssueOperationType.HangUp,
                $"【{common.HangupId.GetNameByEmpId()}】挂起【{common.CreatorId.GetNameByEmpId()}】提出的问题"
            );
        }

        private async Task ReDispatch(InReDispatch input)
        {
            Helper.Helper.CheckInput(input);

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            //Helper.Helper.Assert(input.CurrentAssignment != 0, "转交时必须指定转交人");
            //Helper.Helper.Assert(input.CurrentAssignment != Helper.Helper.GetCurrentUser(), $"当前用户不能是当前问题的转交人");
            //Helper.Helper.Assert(Helper.Helper.GetCurrentUser() == common.Dispatcher
            //    || Helper.Helper.GetCurrentUser() == common.CurrentAssignment, "当前用户不是当前问题的处理人或者分发人，不能进行转交操作");

            input.SetIssue(common);
            await this._issueRep.UpdateNowAsync(common, true);

            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            if (input.SetIssueDetail(issueDetail))
            {
                await this._issueDetailRep.UpdateNowAsync(issueDetail, true);
            }

            //问题转交，如果当前指派（转交人）和当前用户不一致，则发送消息给转交人
            if (common.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                _noticeService.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
            }
        }

        /// <summary>
        /// 转交
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("redispatch")]
        public async Task ReDispatch(List<InReDispatch> input)
        {
            Helper.Helper.CheckInput(input);

            Helper.Helper.Assert(input.Count > 0, "转交信息为空!");

            foreach (var item in input)
            {
                await this.ReDispatch(item);
            }

            await IssueLogger.Log(
                this._issueOperateRep,
                input[0].Id,
                EnumIssueOperationType.ReDispatch,
                $"问题重分发给【{input[0].CurrentAssignment.GetNameByEmpId()}】"
            );
        }

        /// <summary>
        /// 关闭问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("close")]
        public async Task Close(InClose input)
        {
            Helper.Helper.CheckInput(input);
            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            //Helper.Helper.Assert(Helper.Helper.GetCurrentUser() == common.CurrentAssignment, "当前用户不是分发用户，无法执行关闭操作");

            input.SetIssue(common);
            await this._issueRep.UpdateNowAsync(common, true);

            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            if (input.SetIssueDetail(issueDetail))
            {
                await this._issueDetailRep.UpdateNowAsync(issueDetail, true);
            }

            await IssueLogger.Log(
                this._issueOperateRep,
                input.Id,
                EnumIssueOperationType.Close,
                $"【{common.CurrentAssignment.GetNameByEmpId()}】关闭【{common.CreatorId.GetNameByEmpId()}】提出的问题"
            );
        }

        #endregion 流程管理

        #region 分页查询及导出

        /// <summary>
        /// 根据保存的项目id和产品id调用第三方服务获取对应的名称
        /// </summary>
        /// <param name="issues"></param>
        private async Task<PageResult<OutputGeneralIssue>> UpdateProjectProductNames(PageResult<OutputGeneralIssue> issues)
        {
            // 根据保存的项目id和产品id调用第三方服务获取对应的名称
            if (issues.TotalRows > 0)
            {
                IEnumerable<OutputGeneralIssue> projects = issues.Rows.Where<OutputGeneralIssue>(issue => issue.ProjectName != null && issue.ProjectName.StartsWith(Constants.PROJECT_MARK));
                IEnumerable<OutputGeneralIssue> products = issues.Rows.Where<OutputGeneralIssue>(issue => issue.ProductName != null && issue.ProductName.StartsWith(Constants.PRODUCT_MARK));

                if (projects.Any())
                {
                    Dictionary<long, ProjectModelFromThirdParty> projectNames = await Helper.Helper.GetThirdPartyService().GetProjectByIds(projects.Select<OutputGeneralIssue, long>(model => model.ProjectId).Distinct());
                    foreach (var item in projects)
                    {
                        if (projectNames != null && projectNames.ContainsKey(item.ProjectId))
                        {
                            item.ProjectName = projectNames[item.ProjectId].ProjectName;
                        }
                    }
                }

                if (products.Any())
                {
                    Dictionary<long, ProductModelFromThirdParty> productNames = await Helper.Helper.GetThirdPartyService().GetProductByIds(products.Where(model => model.ProductId != null).Select<OutputGeneralIssue, long>(issue => (long)issue.ProductId).Distinct());
                    foreach (var item in products)
                    {
                        long id = (long)item.ProductId;
                        if (productNames != null && productNames.ContainsKey(id))
                        {
                            item.ProductName = productNames[id].ProductName;
                        }
                    }
                }
            }

            return issues;
        }

        private IQueryable<Issue> GetQueryable(BaseQueryModel input)
        {
            IQueryable<Issue> querable = this._issueRep.DetachedEntities
                                     .Where(input.ProjectId != null, u => u.ProjectId == input.ProjectId)
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
                                     .Where(input.SolveTimeTo != null, u => u.SolveTime <= input.SolveTimeTo)

                                     .Where(input.IssueClassification != null, u => u.IssueClassification == input.IssueClassification)
                                     ;

            switch (input.QueryCondition)
            {
                case EnumQueryCondition.Creator:
                    querable = querable.Where(item => item.CreatorId == Helper.Helper.GetCurrentUser());
                    break;

                case EnumQueryCondition.AssignToMe:
                    querable = querable.Where(item => item.CurrentAssignment == Helper.Helper.GetCurrentUser());
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
                    //case EnumQueryCondition.CC:
                    //    querable = querable.Where(item => !string.IsNullOrEmpty(item.CCs) && item.CCs.IndexOf(Helper.Helper.GetCurrentUser().ToString()) > 0);
                    //    break;
            }

            return querable;
        }

        private IQueryable<OutputGeneralIssue> SelectToOutput(BaseQueryModel input, IQueryable<Issue> querable)
        {
            if (input.Module != null)
            {
                if (input.Module == EnumModule.TrialProduce || input.Module == EnumModule.Test)
                {
                    Dictionary<string, FieldStruct> dic = Helper.Helper.GetFieldsStruct(this._issueAttrRep).Result;

                    if (input.Module == EnumModule.TrialProduce && input.TrialProductionProcess != null)
                    {
                        if (dic.ContainsKey(Constants.TRAIL_PRODUCTION))
                        {
                            long fieldId = dic[Constants.TRAIL_PRODUCTION].FieldId;
                            int selectedIndex = (int)input.TrialProductionProcess;

                            return querable.Join(
                                this._issueAttrValueRep.DetachedEntities
                                .Where<IssueExtendAttributeValue>(
                                    model =>
                                    model.AttibuteValue == selectedIndex.ToString()
                                    && model.Id == fieldId
                                ),
                                issue => issue.Id,
                                value => value.IssueNum,
                                (issueModel, attrValModel) => new OutputGeneralIssue(issueModel)
                            );
                        }
                    }

                    if (input.Module == EnumModule.Test && input.TestClassification != null)
                    {
                        if (dic.ContainsKey(Constants.TEST_CALSSIFICATION))
                        {
                            long fieldId = dic[Constants.TEST_CALSSIFICATION].FieldId;
                            int selectedIndex = (int)input.TestClassification;

                            return querable.Join(
                                this._issueAttrValueRep.DetachedEntities
                                .Where<IssueExtendAttributeValue>(
                                    model =>
                                    model.AttibuteValue == selectedIndex.ToString()
                                    && model.Id == fieldId
                                ),
                                issue => issue.Id,
                                value => value.IssueNum,
                                (issueModel, attrValModel) => new OutputGeneralIssue(issueModel)
                            );
                        }
                    }
                }
            }

            return querable.Select(issue => new OutputGeneralIssue(issue));
        }

        /// <summary>
        /// 多种条件分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<PageResult<OutputGeneralIssue>> Page([FromQuery] BaseQueryModel input)
        {
            Helper.Helper.CheckInput(input);

            IQueryable<Issue> querable = this.GetQueryable(input).OrderBy(PageInputOrder.OrderBuilder(input));

            var issues = await this.SelectToOutput(input, querable).ToADPagedListAsync(input.PageNo, input.PageSize);

            issues.Rows.ToList().ForEach(
                delegate (OutputGeneralIssue output)
                {
                    output.BtnList = this.GetBtnList(output.CurrentAssignId, output.Dispatcher, output.Status);
                }
                );

            return await this.UpdateProjectProductNames(issues);
        }

        /// <summary>
        /// 根据保存的项目id和产品id调用第三方服务获取对应的名称
        /// </summary>
        /// <param name="issues"></param>
        private async Task<List<ExportIssueDto>> UpdateProjectProductNames(List<ExportIssueDto> issues)
        {
            // 根据保存的项目id和产品id调用第三方服务获取对应的名称
            if (issues.Count > 0)
            {
                Dictionary<long, ProjectModelFromThirdParty> projects = await Helper.Helper.GetThirdPartyService().GetProjectByIds(issues.Where(model => model.ProductId != null).Select<ExportIssueDto, long>(issue => (long)issue.ProjectId));
                Dictionary<long, ProductModelFromThirdParty> products = await Helper.Helper.GetThirdPartyService().GetProductByIds(issues.Where(model => model.ProductId != null).Select<ExportIssueDto, long>(issue => (long)issue.ProductId));

                foreach (var item in issues)
                {
                    if (projects != null & projects.ContainsKey(item.ProjectId))
                    {
                        item.ProjectName = projects[item.ProjectId].ProjectName;
                    }

                    long id = (long)item.ProductId;
                    if (products != null && products.ContainsKey(id))
                    {
                        item.ProductName = products[id].ProductName;
                    }
                }
            }

            return issues;
        }

        private IQueryable<ExportIssueDto> GetExportQuerable(BaseQueryModel input)
        {
            return this.GetQueryable(input).Join(
                                        this._issueDetailRep.DetachedEntities,
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

                case EnumQueryCondition.AssignToMe:
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
        [HttpPost("export")]
        public async Task<IActionResult> Export(List<long> input)
        {
            Helper.Helper.CheckInput(input);

            Helper.Helper.Assert(input.Count > 0, "没有问题项选中，无法导出");

            // 导出查询到的数据
            //IQueryable<ExportIssueDto> querable = this.GetExportQuerable(input);

            //this.AddFilter(input, querable);

            //PageResult<ExportIssueDto> list = await querable.ToADPagedListAsync(input.PageNo, input.PageSize);

            // 导出前端选中的数据

            List<ExportIssueDto> list = this._issueRep.DetachedEntities.Where<Issue>(issue => input.Contains(issue.Id)).Join(this._issueDetailRep.DetachedEntities, issue => issue.Id,
                                        detailIssue => detailIssue.Id,
                                        (issue, detail) => new ExportIssueDto(issue, detail)).ToList();

            List<ExportIssueDto> colls = await this.UpdateProjectProductNames(list);

            return await Helper.Helper.ExportExcel(colls);
        }

        #endregion 分页查询及导出

        #region 问题数据导入

        /// <summary>
        /// 下载Excel以方便导入问题数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("template")]
        public async Task<IActionResult> Template()
        {
            var item = this._issueRep.Where(model => model.Title != null).Take(1).ProjectToType<InIssue>();
            return await Helper.Helper.ExportExcel(item, "IssueTemplate");
        }

        /// <summary>
        /// 问题数据导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("import")]
        public async Task ImportIssues(IFormFile file)
        {
            Helper.Helper.CheckInput(file);
            Helper.Helper.Assert(!string.IsNullOrEmpty(file.FileName), "文件为空");
            Helper.Helper.Assert(file.FileName, fileName => fileName.Contains("IssueTemplate") && fileName.EndsWith(".xlsx"), "请使用下载的模板进行数据导入");

            var collection = MiniExcel.Query(file.OpenReadStream(), useHeaderRow: true)
                .TakeWhile(item => item.问题简述 != null && item.详情 != null && item.产品线名称 != null && item.项目名称 != null && item.问题模块 != null
                && item.问题分类 != null && item.当前指派人名称 != null && item.问题性质 != null && item.问题来源 != null);

            long userId = CurrentUserInfo.UserId;

            List<string> result = new List<string>();
            List<InIssue> issuList = new List<InIssue>();
            foreach (var item in collection)
            {
                string msg = "";    //记录校验数据的错误信息
                var issue = this.CheckExcelImport(item, out msg);
                if (issue == null)
                {
                    result.Add("【" + Convert.ToString(item.问题简述) + "：" + msg + "】");
                    break;
                }
                else
                {
                    issuList.Add(issue);
                }
            }
            if (result != null && result.Count() > 0)
            {
                string msg = string.Join(",", result);
                throw Oops.Oh("标题为：" + msg + "请检查后重新导入");
            }

            //循环新增
            foreach (var item in issuList)
            {
                await this.Add(item);
            }
        }

        /// <summary>
        /// 业务逻辑上判断导入的Excel数据是否符合要求，符合要求则返回对应的新增数据包
        /// </summary>
        /// <param name="item">excel数据行</param>
        /// <param name="msg">记录校验数据的错误信息</param>
        /// <returns></returns>
        public InIssue CheckExcelImport(dynamic item, out string msg)
        {
            InIssue issue = new InIssue();
            issue = this.CheckExcelIssue(item, out msg);
            if (issue == null)
            {
                return null;
            }
            issue.ExtendAttribute = this.CheckExcelIssueExtAttr(item, out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return null;
            }
            return issue;
        }

        /// <summary>
        /// 业务逻辑上判断导入的Excel的Issue表数据是否符合要求
        /// </summary>
        /// <param name="item">excel数据行</param>
        /// <param name="msg">记录校验数据的错误信息</param>
        /// <returns></returns>
        public InIssue CheckExcelIssue(dynamic item, out string msg)
        {
            string title = Convert.ToString(item.问题简述);
            string description = Convert.ToString(item.详情);
            string productName = Convert.ToString(item.产品线名称);
            string projectName = Convert.ToString(item.项目名称);
            string currentAssignmentName = Convert.ToString(item.当前指派人名称);
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(currentAssignmentName))
            {
                msg = "请问输入问题简述，产品线名称，产品项目名称或当前指派人名称";
                return null;
            }

            //判断项目，产品和当前指派人是否存在
            var productId = productName.GetProductIdByName();
            var projectId = projectName.GetProjectIdByName();
            var currentAssignment = currentAssignmentName.GetEmpIdByName();
            if (productId == 0 || projectId == 0 || currentAssignment == 0)
            {
                msg = "请确保输入正确的产品线名称，产品项目名称或当前指派人名称";
                return null;
            }

            //判断枚举值是否存在
            var modular = (EnumModule)Helper.Helper.GetIntFromEnumDescription(item.问题模块);
            var consequence = (EnumConsequence)Helper.Helper.GetIntFromEnumDescription(item.问题性质);
            var classification = (EnumIssueClassification)Helper.Helper.GetIntFromEnumDescription(item.问题分类);
            var source = (EnumIssueSource)Helper.Helper.GetIntFromEnumDescription(item.问题来源);
            if (modular == (EnumModule)(-1) || consequence == (EnumConsequence)(-1) || classification == (EnumIssueClassification)(-1) || source == (EnumIssueSource)(-1))
            {
                msg = "请输入正确的问题模块名称，问题性质名称，问题分类名称或问题来源名称";
                return null;
            }

            //根据模块值获取该值对应的模块的ID（此处是为了获取到模块和项目的人员交集）
            string modularValue = Convert.ToString(item.问题模块);
            var modularId = modularValue.GetModularIdByValue();
            //判断当前指派是否存在于项目和模块的交集人员列表
            var userList = Helper.Helper.GetUserByProjectModularId(projectId, modularId);
            if (userList == null || userList.Count() <= 0 || !userList.Select(u => u.Id).ToList().Contains(currentAssignment))
            {
                msg = "当前指派人不存在该模块和项目中，请确认";
                return null;
            }

            //判断当前数据是否已经存在于数据库中（录入数据和现有数据完全相同的情况下才能判断到）
            var isExists = _issueRep.DetachedEntities.FirstOrDefault(u => u.ProductId == productId && u.ProjectId == projectId && u.Module == modular && u.Consequence == consequence
            && u.IssueClassification == classification && u.Source == source && u.Title == title);
            if (isExists != null)
            {
                msg = "该数据行已经存在于系统中";
                return null;
            }

            //创建用于新建的数据包
            var issue = new InIssue()
            {
                Title = title,
                Description = description,
                ProductId = productId,
                ProjectId = projectId,
                CurrentAssignment = currentAssignment,
                Module = modular,
                Consequence = consequence,
                IssueClassification = classification,
                Source = source,
                Status = EnumIssueStatus.Closed    //Excel导入数据状态默认为关闭状态，导入后不用重新走流程
            };

            msg = "";
            return issue;
        }

        /// <summary>
        /// 业务逻辑上判断导入的Excel的扩展属性表数据是否符合要求
        /// </summary>
        /// <param name="item">excel数据行</param>
        /// <param name="msg">记录校验数据的错误信息</param>
        /// <returns></returns>
        public string CheckExcelIssueExtAttr(dynamic item, out string msg)
        {
            List<FieldValue> fieldValues = new List<FieldValue>();
            msg = "";
            //获取问题模块下的所有扩展属性
            var modular = (EnumModule)Helper.Helper.GetIntFromEnumDescription(item.问题模块);
            var extendAttr = _issueAttrRep.DetachedEntities.Where(u => u.Module == modular);
            var nameList = extendAttr.Select(u => u.AttibuteName).ToList();
            foreach (dynamic obj in item)
            {
                string key = obj.Key;
                if (nameList.Contains(key))
                {
                    var extendAttribute = extendAttr.FirstOrDefault(u => u.AttibuteName == key);
                    FieldValue value = new FieldValue();
                    value.FieldCode = extendAttribute.AttributeCode;
                    value.FieldDataType = extendAttribute.ValueType;
                    value.Module = extendAttribute.Module;
                    value.FieldId = extendAttribute.Id;
                    value.FieldName = extendAttribute.AttibuteName;
                    switch (value.FieldDataType)
                    {
                        case "Enum":
                            var enumValue = Helper.Helper.GetIntFromEnumDescription(obj.Value);
                            if (enumValue != -1)
                            {
                                value.Value = enumValue;
                            }
                            else
                            {
                                msg += key + "-" + obj.Value + "：输入不正确；";
                            }
                            break;

                        case "DateTime":
                            if (DateTime.TryParse(obj.Value, out DateTime date))
                            {
                                value.Value = date.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                msg += key + "-" + obj.Value + "：日期输入不正确；";
                            }
                            break;
                    }
                    value.Value = obj.Value;
                    fieldValues.Add(value);
                }
            }

            //如果是试产模块则再默认增加问题性质评分一项
            if (modular == EnumModule.TrialProduce)
            {
                FieldValue value = new FieldValue();
                value.FieldCode = "ImpactScore";
                value.FieldDataType = "decimal";
                value.Module = EnumModule.TrialProduce;
                value.FieldId = 76;
                value.FieldName = "问题性质评分";

                var consequence = (EnumConsequence)Helper.Helper.GetIntFromEnumDescription(item.问题性质);
                switch (consequence)
                {
                    case EnumConsequence.Deadly:
                        value.Value = "10";
                        break;

                    case EnumConsequence.Serious:
                        value.Value = "3";
                        break;

                    case EnumConsequence.General:
                        value.Value = "1";
                        break;

                    case EnumConsequence.Prompt:
                        value.Value = "0.3";
                        break;
                }
            }

            if (fieldValues.Count() <= 0)
            {
                return "";
            }
            return JSON.Serialize(fieldValues);
        }

        #endregion 问题数据导入

        #region 问题相关附件的信息保存和获取

        public class AttachmentIssue
        {
            public List<AttachmentModel> Attachments { get; set; }

            /// <summary>
            /// 问题编号
            /// </summary>
            public long IssueId { get; set; }
        }

        /// <summary>
        /// 附件上传后通知问题表保存附件id
        /// 20220511 逻辑改动：由原来的后端上传并保存Id信息改为前端上传、后端保存Id
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("attachment/saveId")]
        public async Task SaveAttachment(AttachmentIssue input)
        {
            Helper.Helper.CheckInput(input);

            Helper.Helper.Assert(input.Attachments != null && input.Attachments.Count > 0 && input.IssueId != 0, "附件信息或问题编号为空!无法保存");

            IssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.IssueId);

            var list = new List<AttachmentModel>();
            if (!string.IsNullOrEmpty(detail.Attachments))
            {
                list = JsonConvert.DeserializeObject<List<AttachmentModel>>(detail.Attachments);
            }

            list.AddRange(input.Attachments.Where(model => !list.Select<AttachmentModel, long>(attach => attach.AttachmentId).Contains(model.AttachmentId)));

            detail.Attachments = JsonConvert.SerializeObject(list);

            await this._issueDetailRep.UpdateIncludeAsync(detail, new string[] { nameof(detail.Attachments) }, true);

            await IssueLogger.Log(this._issueOperateRep, input.IssueId, EnumIssueOperationType.Upload, JSON.Serialize(input.Attachments));
        }

        /// <summary>
        /// 附件下载时获取该问题所属的附件编号等信息
        /// 传入问题编号
        /// </summary>
        /// <returns></returns>
        [HttpPost("attachment/infoList")]
        public async Task<List<AttachmentModel>> AttachmentInfoList(BaseId input)
        {
            Helper.Helper.CheckInput(input);

            Helper.Helper.Assert(input.Id != 0, "问题编号不合法,无法获取相关附件信息");

            IssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);

            List<AttachmentModel> list = null;
            if (!string.IsNullOrEmpty(detail.Attachments))
            {
                list = JsonConvert.DeserializeObject<List<AttachmentModel>>(detail.Attachments);
            }

            return list;
        }

        #endregion 问题相关附件的信息保存和获取

        #region 分发操作

        /// <summary>
        /// 分派
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("dispatch")]
        public async Task Dispatch(InDispatch input)
        {
            Helper.Helper.CheckInput(input);

            Issue issue = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            Helper.Helper.Assert(issue.CurrentAssignment != null && issue.CurrentAssignment == Helper.Helper.GetCurrentUser(), "当前指派人不是当前用户(分发人)");
            Helper.Helper.Assert(input.CurrentAssignment != null && input.CurrentAssignment != 0, "分发时必须指定执行人");

            input.SetIssue(issue);
            await this._issueRep.UpdateNowAsync(issue);

            IssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            input.SetIssueDetail(detail);
            await this._issueDetailRep.UpdateNowAsync(detail, null);

            if (!string.IsNullOrEmpty(input.ExtendAttribute))
            {
                // 新增扩展属性时
                List<FieldValue> list = JSON.Deserialize<List<FieldValue>>(input.ExtendAttribute);

                foreach (var item in list)
                {
                    item.IssueId = input.Id;
                }

                var attrs = JSON.Serialize(list);

                await this.UpdateAttributeValuesBatch(attrs);
            }
            else
            {
                if (this._issueAttrValueRep.Any(val => val.IssueNum == input.Id))
                {
                    await this._issueAttrValueRep.DeleteNowAsync(this._issueAttrValueRep.Where(val => val.IssueNum == input.Id));
                }
            }

            //如果当前用户和解决人不一致，则发送消息给解决人
            if (issue.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                _noticeService.SendNotice(issue.Id.ToString(), issue.CurrentAssignment.ToString(), issue.Title);
            }

            await IssueLogger.Log(
                this._issueOperateRep,
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

                Helper.Helper.Assert(list != null && list.Count > 0, "更新扩展字段数据时，数据为空");


                foreach (var item in list)
                {
                    var model = new IssueExtendAttributeValue()
                    {
                        Id = item.FieldId,
                        IssueNum = item.IssueId,
                        AttibuteValue = item.Value
                    };

                    if (this._issueAttrValueRep.Any(model => model.Id == item.FieldId && model.IssueNum == item.IssueId))
                    {
                        await this._issueAttrValueRep.UpdateNowAsync(model);
                    }
                    else
                    {
                        await this._issueAttrValueRep.InsertNowAsync(model);
                    }
                }

                //IEnumerable<long> attrIds = list.Select<FieldValue, long>(model => model.FieldId);
                //IEnumerable<long> issueIds = list.Select<FieldValue, long>(model => model.IssueId);

                //IEnumerable<IssueExtendAttributeValue> values = this._issueAttrValueRep.Where(
                //    model =>
                //    model.IssueNum == list[0].IssueId
                //    && attrIds.Contains(model.Id)
                //);

                //int realCount = list.Count;

                //if (realCount > values.Count())
                //{
                //    IEnumerable<long> ids = values.Select<IssueExtendAttributeValue, long>(val => val.Id);
                //    if (ids != null && ids.Any())
                //    {
                //        IssueExtendAttributeValue[] insertLists = list.TakeWhile(model => !ids.Contains(model.FieldId))?.Select<FieldValue, IssueExtendAttributeValue>(model => new IssueExtendAttributeValue
                //        {
                //            Id = model.FieldId,
                //            IssueNum = model.IssueId,
                //            AttibuteValue = model.Value
                //        })?.ToArray();

                //        if (insertLists != null && insertLists.Any())
                //        {
                //            this._issueAttrValueRep.Entities.AddRange(insertLists);

                //            this._issueAttrValueRep.Context.SaveChanges();

                //            list.RemoveAll(model => ids.Contains(model.FieldId));
                //        }
                //    }
                //}

                //this._issueAttrValueRep.Entities.UpdateRange(list.Select(model => new IssueExtendAttributeValue
                //{
                //    Id = model.FieldId,
                //    IssueNum = model.IssueId,
                //    AttibuteValue = model.Value
                //}));

                //await this._issueAttrValueRep.Context.SaveChangesAsync();
            }
        }

        #endregion 分发操作

        #region 问题列表列名管理

        /// <summary>
        /// 获取问题列表列名
        /// </summary>
        /// <param name="input">{"id":"序号","title":"标题"}</param>
        /// <returns></returns>
        [HttpGet("column/display")]
        public async Task<Dictionary<string, string>> GetColumnDisplay()
        {
            return await Helper.Helper.GetUserColumns(this._issueColumnDisplayRep);
        }

        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="input">{"id":"序号","title":"标题"}</param>
        /// <returns></returns>
        [HttpPost("column/update")]
        public async Task UpdateColumnDisplay(Dictionary<string, string> input)
        {
            Helper.Helper.CheckInput(input);

            await Helper.Helper.SetUserColumns(this._issueColumnDisplayRep, JSON.Serialize(input));
        }

        #endregion 问题列表列名管理

        #region 问题统计

        /// <summary>
        /// 问题统计数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("statistic")]
        public async Task<IEnumerable<DataPairOutput>> Statistic(StatisticInput input)
        {
            Helper.Helper.CheckInput(input);
            Helper.Helper.Assert((input.To - input.From < TimeSpan.FromDays(365)), "间隔不允许超过一年");

            var collections = await this._issueRep.Where(model => model.CreateTime >= input.From && model.CreateTime <= input.To)
                .OrderBy<Issue, DateTime>(model => model.CreateTime)
                .ProjectToType<IssuePropertyDto>().ToListAsync();

            IEnumerable<DataPairOutput> results = collections.GroupBy<IssuePropertyDto, DateTime>(model => model.CreateTime.Date)
                .Select<IGrouping<DateTime, IssuePropertyDto>, DataPairOutput>(group => new DataPairOutput(group.Key, new StatisticData()
                {
                    DeadlyConsequenceCount = group.Count(model => model.Consequence == EnumConsequence.Deadly),
                    SeriousConsequenceCount = group.Count(model => model.Consequence == EnumConsequence.Serious),
                    AssignToMeCount = group.Count(model => model.CurrentAssignment == Helper.Helper.GetCurrentUser()),
                    SolvedCount = group.Count(model => model.Status == EnumIssueStatus.Solved)
                }));

            var list = new List<DataPairOutput>();

            DateTime from = input.From.Date;
            DateTime to = input.To.Date;

            DateTime current = from;

            foreach (var item in results)
            {
                while (item.Day > current)
                {
                    list.Add(new DataPairOutput(current));
                    current = current.AddDays(1);
                }

                list.Add(item);
                current = current.AddDays(1);
            }

            while (current <= to)
            {
                list.Add(new DataPairOutput(current));
                current = current.AddDays(1);
            }

            return list;
        }

        #endregion 问题统计

        #region 问题催办

        /// <summary>
        /// 对某个问题发送催办消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("sendurgenotice")]
        public async Task SendUrgeNotice(BaseId input)
        {
            Helper.Helper.CheckInput(input);
            Issue issue = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            Helper.Helper.Assert(issue.CurrentAssignment != null && issue.CurrentAssignment != 0, "当前问题的当前指派人为空");

            _noticeService.SendNotice(issue.Id.ToString(), issue.CurrentAssignment.ToString(), issue.Title);

            await IssueLogger.Log(
                this._issueOperateRep,
                input.Id,
                EnumIssueOperationType.Dispatch,
                $"当前用户【{Helper.Helper.GetCurrentUser()}】向【{issue.Id}】-【{issue.Title}】发送催办消息"
            );
        }

        /// <summary>
        /// 批量发送催办消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendUrgeNoticeList(List<BaseId> input)
        {
            foreach (BaseId id in input)
            {
                await this.SendUrgeNotice(id);
            }
        }

        /// <summary>
        /// 定时发送消息
        /// </summary>
        /// <returns></returns>
        [HttpGet("sendtimingurgenotice")]
        public async Task SendTimingUrgeNotice()
        {
            //获取所有预计完成时间小于等于当前当前时间的问题
            var list = _issueRep.DetachedEntities.Where(u => u.Status == EnumIssueStatus.UnSolve || u.Status == EnumIssueStatus.Created)
                .Where(u => u.CurrentAssignment != null && u.CurrentAssignment != 0)
                .Where(u => Convert.ToInt32(u.ForecastSolveTime.Value.ToString("yyyyMMdd")) <= Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")))
                .Where(u => u.IsDeleted == false)
                .Select(u => u.Id);
            foreach (long id in list)
            {
                BaseId input = new BaseId() { Id = id };
                Issue issue = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

                _noticeService.SendNotice(issue.Id.ToString(), issue.CurrentAssignment.ToString(), issue.Title);

                await IssueLogger.Log(
                    this._issueOperateRep,
                    input.Id,
                    EnumIssueOperationType.Dispatch,
                    $"定时任务向【{issue.Id}】-【{issue.Title}】发送催办消息"
                );
            }
        }

        #endregion 问题催办

        #region 问题按钮显示获取

        /// <summary>
        /// 后台判断问题状态，当前登录用户，是否问题管理者下的按钮显示问题
        /// </summary>
        /// <param name="issue">当前问题</param>
        /// <returns></returns>
        public List<EnumIssueButton> GetBtnList(Issue issue)
        {
            return this.GetBtnList(issue.CurrentAssignment, issue.Dispatcher, issue.Status);
        }

        /// <summary>
        /// 后台判断问题状态，当前登录用户，是否问题管理者下的按钮显示问题
        /// </summary>
        /// <param name="currentAssignment">当前指派</param>
        /// <param name="dispatcher">分发人(问题管理者)</param>
        /// <param name="status">问题状态</param>
        /// <returns></returns>
        public List<EnumIssueButton> GetBtnList(long? currentAssignment, long? dispatcher, EnumIssueStatus status)
        {
            //当前登录用户
            var curUserId = CurrentUserInfo.UserId;
            var result = new List<EnumIssueButton>();
            result.Add(EnumIssueButton.Copy);
            result.Add(EnumIssueButton.Detail);

            //超级管理员才能进行删除动作
            var adminType = CurrentUserInfo.IsSuperAdmin;
            if (adminType)
            {
                result.Add(EnumIssueButton.Delete);
            }
            if (currentAssignment == null || currentAssignment == 0)
            {
                return result;
            }
            //分发人为空的时候，则意味着这是新建状态下，此时分发人=当前指派
            if (dispatcher == null || dispatcher == 0)
            {
                dispatcher = currentAssignment;
            }
            //当前用户==当前指派，当前用户==问题管理者
            if (curUserId == currentAssignment && curUserId == dispatcher)
            {
                switch (status)
                {
                    case EnumIssueStatus.Created:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        result.Add(EnumIssueButton.Dispatch);
                        break;

                    case EnumIssueStatus.Dispatched:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        result.Add(EnumIssueButton.Execute);
                        break;

                    case EnumIssueStatus.Solved:
                        result.Add(EnumIssueButton.Edit);
                        //result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        result.Add(EnumIssueButton.ReCheck);
                        break;

                    case EnumIssueStatus.UnSolve:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        result.Add(EnumIssueButton.Execute);
                        break;

                    case EnumIssueStatus.Closed:
                        result.Add(EnumIssueButton.ReOpen);
                        break;

                    case EnumIssueStatus.HasHangUp:
                        result.Add(EnumIssueButton.ReOpen);
                        break;

                    case EnumIssueStatus.HasTemporary:
                        result.Add(EnumIssueButton.Edit);
                        break;

                    case EnumIssueStatus.HasRechecked:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        result.Add(EnumIssueButton.Validate);
                        break;
                }
            }
            //当前用户==当前指派，当前用户!=问题管理者
            else if (curUserId == currentAssignment && curUserId != dispatcher)
            {
                switch (status)
                {
                    case EnumIssueStatus.Created:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        result.Add(EnumIssueButton.Dispatch);
                        break;

                    case EnumIssueStatus.Dispatched:
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.Execute);
                        break;

                    case EnumIssueStatus.Solved:
                        result.Add(EnumIssueButton.HangUp);
                        break;

                    case EnumIssueStatus.UnSolve:
                        //result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.Execute);
                        break;

                    case EnumIssueStatus.Closed:
                        break;

                    case EnumIssueStatus.HasHangUp:
                        result.Add(EnumIssueButton.ReOpen);
                        break;

                    case EnumIssueStatus.HasTemporary:
                        result.Add(EnumIssueButton.Edit);
                        break;

                    case EnumIssueStatus.HasRechecked:
                        result.Add(EnumIssueButton.Validate);
                        break;
                }
            }
            //当前用户!=当前指派，当前用户==问题管理者
            else if (curUserId != currentAssignment && curUserId == dispatcher)
            {
                switch (status)
                {
                    case EnumIssueStatus.Created:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        result.Add(EnumIssueButton.Dispatch);
                        break;

                    case EnumIssueStatus.Dispatched:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        break;

                    case EnumIssueStatus.Solved:
                        result.Add(EnumIssueButton.Edit);
                        //result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        break;

                    case EnumIssueStatus.UnSolve:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        break;

                    case EnumIssueStatus.Closed:
                        result.Add(EnumIssueButton.ReOpen);
                        break;

                    case EnumIssueStatus.HasHangUp:
                        result.Add(EnumIssueButton.ReOpen);
                        break;

                    case EnumIssueStatus.HasTemporary:
                        result.Add(EnumIssueButton.Edit);
                        break;

                    case EnumIssueStatus.HasRechecked:
                        result.Add(EnumIssueButton.Edit);
                        result.Add(EnumIssueButton.ReDispatch);
                        result.Add(EnumIssueButton.HangUp);
                        result.Add(EnumIssueButton.Close);
                        result.Add(EnumIssueButton.Notice);
                        break;
                }
            }
            //当前用户!=当前指派，当前用户!=问题管理者
            else if (curUserId != currentAssignment && curUserId != dispatcher)
            {
                switch (status)
                {
                    case EnumIssueStatus.Created:
                        break;

                    case EnumIssueStatus.Dispatched:
                        break;

                    case EnumIssueStatus.Solved:
                        break;

                    case EnumIssueStatus.UnSolve:
                        break;

                    case EnumIssueStatus.Closed:
                        break;

                    case EnumIssueStatus.HasHangUp:
                        break;

                    case EnumIssueStatus.HasTemporary:
                        break;

                    case EnumIssueStatus.HasRechecked:
                        break;
                }
            }
            return result.Distinct().ToList();
        }

        #endregion 问题按钮显示获取

        #region 获取问题序号
        public static string GetTenantId()
        {
            if (App.User == null) return string.Empty;
            return App.User.FindFirst(ClaimConst.TENANT_ID)?.Value + "_";
        }
        /// <summary>
        /// 获取新的问题序号
        /// </summary>
        /// <returns></returns>
        public string GetNewSerialNumber(EnumModule modular)
        {
            System.Object locker = new System.Object();
            lock (locker)
            {
                int seedNum = 0;
                string abbreviation = "";
                switch (modular)
                {
                    case EnumModule.R_D:
                        abbreviation = "R&D";
                        break;

                    case EnumModule.Test:
                        abbreviation = "TST";
                        break;

                    case EnumModule.TrialProduce:
                        abbreviation = "NPI";
                        break;

                    case EnumModule.IQC:
                        abbreviation = "IQC";
                        break;

                    case EnumModule.MassProduction:
                        abbreviation = "MSP";
                        break;

                    case EnumModule.AfterSales:
                        abbreviation = "AMS";
                        break;
                }

                //缓存Key值，如NPI_20220622
                var key = GetTenantId() + "_" + abbreviation + "_" + DateTime.Now.ToString("yyyyMMdd");
                string result = _issueCacheService.GetString(key).Result;
                if (string.IsNullOrEmpty(result))
                {
                    result = _issueRep.DetachedEntities.OrderByDescending(u => u.CreateTime).FirstOrDefault(u => u.Module == modular && u.CreateTime.Date == DateTime.Now.Date)?.SerialNumber;
                    if (string.IsNullOrEmpty(result))
                    {
                        result = abbreviation + DateTime.Now.ToString("yyyyMMdd") + seedNum.ToString("000");
                    }
                    //截取字符串中的日期部分
                    if (result.Substring(3, 8) != DateTime.Now.ToString("yyyyMMdd"))
                    {
                        result = abbreviation + DateTime.Now.ToString("yyyyMMdd") + seedNum.ToString("000");
                    }
                }
                int.TryParse(result.Substring(result.Length - 3, 3), out seedNum);
                seedNum++;
                result = abbreviation + DateTime.Now.ToString("yyyyMMdd") + seedNum.ToString("000");
                _issueCacheService.SetString(key, result, 12, 0, 0).Wait();
                return result;
            }
        }

        #endregion 获取问题序号
    }
}