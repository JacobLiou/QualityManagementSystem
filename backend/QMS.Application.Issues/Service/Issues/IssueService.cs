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

        private readonly string ProblemInfoUrl = "http://qms.sofarsolar.com:8002/problemInfo?id=";


        private readonly string ProblemInfoTitle = "质量平台问题管理";
        private readonly string ProblemInfoContent = "您好，您当前有个问题需要关注，请登录质量平台查看";

        public IssueService(
            IRepository<Issue, IssuesDbContextLocator> issueRep,
            IRepository<IssueDetail, IssuesDbContextLocator> issueDetailRep,
            IRepository<IssueOperation, IssuesDbContextLocator> issueOperateRep,
            IRepository<IssueExtendAttribute, IssuesDbContextLocator> issueAttrRep,
            IRepository<IssueExtendAttributeValue, IssuesDbContextLocator> issueAttrValueRep,
            IRepository<IssueColumnDisplay, IssuesDbContextLocator> issueColumnDisplayRep

        )
        {
            this._issueRep = issueRep;
            this._issueDetailRep = issueDetailRep;
            this._issueOperateRep = issueOperateRep;
            this._issueAttrRep = issueAttrRep;
            this._issueAttrValueRep = issueAttrValueRep;

            this._issueColumnDisplayRep = issueColumnDisplayRep;
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
                this.SendNotice(issueEntity.Entity.Id.ToString(), input.CurrentAssignment.ToString(), issueEntity.Entity.Title);
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
                this.SendNotice(issue.Id.ToString(), input.CurrentAssignment.ToString(), issue.Title);
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
            Helper.Helper.Assert(common.Status == EnumIssueStatus.HasHangUp || common.Status == EnumIssueStatus.Closed, "必须为已挂起或者已关闭状态才能开启");
            Helper.Helper.Assert(common.CurrentAssignment == Helper.Helper.GetCurrentUser(), "必须为问题分发人才能重开启问题");

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

            Helper.Helper.Assert(common.CurrentAssignment != null && common.CurrentAssignment == Helper.Helper.GetCurrentUser(), $"当前指派人不是当前用户(复核人)");

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
                this.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
            }
            //问题复核无效，则问题重新流转至解决，如果当前指派（解决人）和当前用户不一致则发送消息给解决人
            if (!pass && common.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                this.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
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

            Helper.Helper.Assert(common.CurrentAssignment != null && common.CurrentAssignment == Helper.Helper.GetCurrentUser(), $"当前指派人不是当前用户(执行人)");

            input.SetIssue(common);
            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            input.SetIssueDetail(issueDetail);

            await this._issueRep.UpdateNowAsync(common, true);

            await this._issueDetailRep.UpdateNowAsync(issueDetail, true);

            //如果当前用户和复核人（分发人）不一致，则发送消息给复核人（分发人）
            if (common.CurrentAssignment != Helper.Helper.GetCurrentUser())
            {
                this.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
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
                this.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
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

            Helper.Helper.Assert(input.CurrentAssignment != 0, "转交时必须指定转交人");
            Helper.Helper.Assert(input.CurrentAssignment != Helper.Helper.GetCurrentUser(), $"当前用户不能是当前问题的转交人");
            Helper.Helper.Assert(Helper.Helper.GetCurrentUser() == common.Dispatcher
                || Helper.Helper.GetCurrentUser() == common.CurrentAssignment, "当前用户不是当前问题的处理人或者分发人，不能进行转交操作");

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
                this.SendNotice(common.Id.ToString(), common.CurrentAssignment.ToString(), common.Title);
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

            Helper.Helper.Assert(Helper.Helper.GetCurrentUser() == common.CurrentAssignment, "当前用户不是分发用户，无法执行关闭操作");

            input.SetIssue(common);

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
                Dictionary<long, ProjectModelFromThirdParty> projects = await Helper.Helper.GetThirdPartyService().GetProjectByIds(issues.Select<ExportIssueDto, long>(issue => issue.ProjectId));
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

            IEnumerable<dynamic> collection = MiniExcel.Query(file.OpenReadStream(), true)
                .TakeWhile(item => item.标题 != null && item.问题模块 != null && item.问题性质 != null && item.问题分类 != null && item.问题来源 != null);

            long userId = CurrentUserInfo.UserId;

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
                    //Dispatcher = Convert.ToInt64(item.分发人编号),
                    Source = (EnumIssueSource)Helper.Helper.GetIntFromEnumDescription(item.问题来源),
                    Discover = Convert.ToInt64(item.发现人编号),
                    DiscoverTime = Convert.ToDateTime(item.发现日期),
                    Status = EnumIssueStatus.Created,
                    //CreatorId = item.提出人编号 == null ? userId : Convert.ToInt64(item.提出人编号),
                    //CreateTime = Convert.ToDateTime(item.提出日期),
                    //CC = Convert.ToInt64(item.被抄送人编号)
                };

                await this.Add(issue);
            }
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
                this.SendNotice(issue.Id.ToString(), issue.CurrentAssignment.ToString(), issue.Title);
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

        #region 问题流转发送消息

        /// <summary>
        /// 发送问题消息
        /// </summary>
        /// <param name="issueId">问题详情ID</param>
        /// <param name="userId">接收消息用户</param>
        /// <returns></returns>
        public async Task SendNotice(string issueId, string userId, string title)
        {
            //构建消息格式
            var msg = new NoticeMsgInput()
            {
                Url = ProblemInfoUrl + issueId,
                Content = ProblemInfoContent,
                Title = ProblemInfoTitle + "-" + title,
                UserIdList = new List<string>() { userId }
            };
            if (!string.IsNullOrEmpty(msg.Url))
            {
                App.GetService<IssueStatusNoticeService>().SendNoticeAsync(msg);
            }
        }

        #endregion 问题流转发送消息
    }
}