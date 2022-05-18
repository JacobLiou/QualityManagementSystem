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
using QMS.Application.Issues.Service.Issues.Dto.Update;
using QMS.Application.Issues.Service.ThirdPartyService.Dto;
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
    public class IssueService : IIssueService, IDynamicApiController, ITransient
    {
        private readonly IRepository<Issue, IssuesDbContextLocator> _issueRep;
        private readonly IRepository<IssueDetail, IssuesDbContextLocator> _issueDetailRep;
        private readonly IRepository<IssueOperation, IssuesDbContextLocator> _issueOperateRep;
        private readonly IRepository<IssueExtendAttribute, IssuesDbContextLocator> _issueAttrRep;
        private readonly IRepository<IssueExtendAttributeValue, IssuesDbContextLocator> _issueAttrValueRep;

        private readonly IRepository<IssueColumnDisplay, IssuesDbContextLocator> _issueColumnDisplayRep;

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
        [HttpPost("/issue/add")]
        public async Task<BaseId> Add(InIssue input)
        {
            var issue = input.Adapt<Issue>();
            if (input.CCList != null && input.CCList.Count > 0)
            {
                issue.CCs = JSON.Serialize(input.CCList);
            }
            Helper.Helper.Assert(issue.Dispatcher != null, "创建问题时必须要指定分发者!");
            issue.SetCreate();

            EntityEntry<Issue> issueEntity = await this._issueRep.InsertNowAsync(issue, ignoreNullValues: true);

            var detail = input.Adapt<IssueDetail>();
            detail.Id = issueEntity.Entity.Id;

            await this._issueDetailRep.InsertNowAsync(detail, ignoreNullValues: true);

            // 插入扩展字段数据
            await this.AddAttributeValuesBatch(input.ExtendAttribute);

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
        [HttpPost("/issue/delete")]
        public async Task Delete(DeleteIssueInput input)
        {
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
        [HttpPost("/issue/edit")]
        public async Task Edit(UpdateIssueInput input)
        {
            Issue issue = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            //var isExist = await _issueRep.AnyAsync(u => u.Id == input.Id, false);
            //if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            //issue = input.Adapt<issue>();

            // 手动更新，防止已有数据丢失
            input.SetIssue(issue);
            await _issueRep.UpdateNowAsync(issue, ignoreNullValues: true);

            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);

            input.SetIssueDetail(issueDetail);
            await this._issueDetailRep.UpdateNowAsync(issueDetail, true);

            await IssueLogger.Log(
                this._issueOperateRep,
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
            return (await this._issueDetailRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<OutputDetailIssue>();
        }
        #endregion

        #region 流程管理
        /// <summary>
        /// 挂起后重新开启问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/reopen")]
        public async Task ReOpen(BaseId input)
        {
            Helper.Helper.Assert(input != null, Oops.Oh(ErrorCode.xg1002));

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            Helper.Helper.Assert(common.Status==EnumIssueStatus.HasHangUp, "必须为已挂起状态才能开启");

            common.DoReOpen();
            await this._issueRep.UpdateNowAsync(common, true);

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
        [HttpPost("/issue/recheck")]
        public async Task ReCheck(InReCheck input)
        {
            Helper.Helper.Assert(input != null, Oops.Oh(ErrorCode.xg1002));

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            bool pass = input.PassResult == YesOrNot.Y;
            input.SetIssue(common);
            common.DoReCheck(pass);
            await this._issueRep.UpdateNowAsync(common, true);

            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            if (input.SetIssueDetail(issueDetail))
            {
                await this._issueDetailRep.UpdateNowAsync(issueDetail, true);
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
        [HttpPost("/issue/execute")]
        public async Task Execute(InSolve input)
        {
            Helper.Helper.Assert(input != null, Oops.Oh(ErrorCode.xg1002));

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            input.SetIssue(common);
            common.DoSolve();


            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            input.SetIssueDetail(issueDetail);

            await this._issueRep.UpdateNowAsync(common, true);

            await this._issueDetailRep.UpdateNowAsync(issueDetail, true);

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
        [HttpPost("/issue/validate")]
        public async Task Validate(InValidate input)
        {
            Helper.Helper.Assert(input != null, Oops.Oh(ErrorCode.xg1002));

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            bool pass = input.PassResult == YesOrNot.Y;
            input.SetIssue(common);
            common.DoVerify(pass);
            await this._issueRep.UpdateNowAsync(common, true);


            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            input.SetIssueDetail(issueDetail);
            await this._issueDetailRep.UpdateNowAsync(issueDetail, true);


            EnumIssueOperationType enumIssueOperationType = pass ? EnumIssueOperationType.Close : EnumIssueOperationType.NoPass;
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
        [HttpPost("/issue/hangup")]
        public async Task HangUp(InHangup input)
        {
            Helper.Helper.Assert(input != null, Oops.Oh(ErrorCode.xg1002));

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            input.SetIssue(common);
            common.SetHangup();


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
            Helper.Helper.Assert(input != null, Oops.Oh(ErrorCode.xg1002));

            Issue common = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);

            input.SetIssue(common);
            Helper.Helper.Assert(common.Executor != null, "重分发时必须指定执行人");
            common.DoDispatch();
            await this._issueRep.UpdateNowAsync(common, true);

            IssueDetail issueDetail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            if (input.SetIssueDetail(issueDetail))
            {
                await this._issueDetailRep.UpdateNowAsync(issueDetail, true);
            }
        }

        /// <summary>
        /// 转交
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/redispatch")]
        public async Task ReDispatch(List<InReDispatch> input)
        {
            Helper.Helper.Assert(input != null && input.Count > 0, "转交信息为空!");

            foreach (var item in input)
            {
                await this.ReDispatch(item);
            }

            await IssueLogger.Log(
                this._issueOperateRep,
                input[0].Id,
                EnumIssueOperationType.ReDispatch,
                $"问题重分发给【{input[0].Executor.GetNameByEmpId()}】"
            );
        }
        #endregion

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
                Dictionary<long, ProjectModelFromThirdParty> projects = await Helper.Helper.GetThirdPartyService().GetProjectByIds(issues.Rows.Select<OutputGeneralIssue, long>(issue => issue.ProjectId));
                Dictionary<long, ProductModelFromThirdParty> products = await Helper.Helper.GetThirdPartyService().GetProductByIds(issues.Rows.Select<OutputGeneralIssue, long>(issue => issue.ProductId));

                foreach (var item in issues.Rows)
                {
                    if (projects != null & projects.ContainsKey(item.ProjectId))
                    {
                        item.ProjectName = projects[item.ProjectId].ProjectName;
                    }

                    if (products != null && products.ContainsKey(item.ProductId))
                    {
                        item.ProductName = products[item.ProductId].ProductName;
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
        [HttpGet("/issue/page")]
        public async Task<PageResult<OutputGeneralIssue>> Page([FromQuery] BaseQueryModel input)
        {
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
                Dictionary<long, ProductModelFromThirdParty> products = await Helper.Helper.GetThirdPartyService().GetProductByIds(issues.Select<ExportIssueDto, long>(issue => issue.ProductId));

                foreach (var item in issues)
                {
                    if (projects != null & projects.ContainsKey(item.ProjectId))
                    {
                        item.ProjectName = projects[item.ProjectId].ProjectName;
                    }

                    if (products != null && products.ContainsKey(item.ProductId))
                    {
                        item.ProductName = products[item.ProductId].ProductName;
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
        [HttpPost("/issue/export")]
        public async Task<IActionResult> Export(List<long> input)
        {
            Helper.Helper.Assert(input != null && input.Count > 0, Oops.Oh(ErrorCode.xg1002));

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
        #endregion

        #region 问题数据导入
        /// <summary>
        /// 下载Excel以方便导入问题数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("/issue/template")]
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
        [HttpPost("/issue/import")]
        public async Task ImportIssues(IFormFile file)
        {
            Helper.Helper.Assert(file != null && !string.IsNullOrEmpty(file.FileName), "文件为空");

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
                    Dispatcher = Convert.ToInt64(item.分发人编号),
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
        #endregion

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
        [HttpPost("/issue/attachment/saveId")]
        public async Task SaveAttachment(AttachmentIssue input)
        {
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
        [HttpPost("/issue/attachment/infoList")]
        public async Task<List<AttachmentModel>> AttachmentInfoList(BaseId issueId)
        {
            Helper.Helper.Assert(issueId != null && issueId.Id != 0, "问题编号不合法,无法获取相关附件信息");

            IssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, issueId.Id);

            List<AttachmentModel> list = null;
            if (!string.IsNullOrEmpty(detail.Attachments))
            {
                list = JsonConvert.DeserializeObject<List<AttachmentModel>>(detail.Attachments);
            }

            return list;
        }
        #endregion

        #region 分发操作
        /// <summary>
        /// 分派
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/dispatch")]
        public async Task Dispatch(InDispatch input)
        {
            Issue issue = await Helper.Helper.CheckIssueExist(this._issueRep, input.Id);
            input.SetIssue(issue);
            Helper.Helper.Assert(issue.Executor != null, "分发时必须指定执行人");
            issue.DoDispatch();
            await this._issueRep.UpdateNowAsync(issue);


            IssueDetail detail = await Helper.Helper.CheckIssueDetailExist(this._issueDetailRep, input.Id);
            await this._issueDetailRep.UpdateNowAsync(detail, null);

            await this.UpdateAttributeValuesBatch(input.ExtendAttribute);

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

                this._issueAttrValueRep.Entities.UpdateRange(list.Select(model => new IssueExtendAttributeValue
                {
                    Id = model.FieldId,
                    IssueNum = model.IssueId,
                    AttibuteValue = model.Value
                }));

                await this._issueAttrValueRep.Context.SaveChangesAsync();
            }
        }
        #endregion

        #region 问题列表列名管理
        /// <summary>
        /// 获取问题列表列名
        /// </summary>
        /// <param name="input">{"id":"序号","title":"标题"}</param>
        /// <returns></returns>
        [HttpGet("/issue/column/display")]
        public async Task<Dictionary<string, string>> GetColumnDisplay()
        {
            return await Helper.Helper.GetUserColumns(this._issueColumnDisplayRep);
        }

        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="input">{"id":"序号","title":"标题"}</param>
        /// <returns></returns>
        [HttpPost("/issue/column/update")]
        public async Task UpdateColumnDisplay(Dictionary<string, string> input)
        {
            Helper.Helper.Assert(input != null, Oops.Oh(ErrorCode.xg1002));

            await Helper.Helper.SetUserColumns(this._issueColumnDisplayRep, JSON.Serialize(input));
        }
        #endregion
    }
}
