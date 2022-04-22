using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.IssueService.Dto.Detail;
using QMS.Application.Issues.IssueService.Dto.Field;
using QMS.Application.Issues.IssueService.Dto.New;
using QMS.Application.Issues.IssueService.Dto.QueryList;
using QMS.Core.Enum;

namespace QMS.Application.IssueService
{
    public interface IMyIssuesService
    {
        /// <summary>
        /// 公共新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddCommon(InCommonNewWithOutId input);

        public Task AddDetail(OutputDetailIssue input);

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="fileds"></param>
        /// <returns></returns>
        Task AddFieldStruct(long creatorId, EnumModule module, List<FieldStruct> fileds);

        /// <summary>
        /// 更新问题字段值
        /// </summary>
        /// <param name="IssueId"></param>
        /// <param name="fieldValues"></param>
        /// <returns></returns>
        Task AddFieldValue(long IssueId, List<FieldValue> fieldValues);

        Task Delete(BaseId input);

        Task Update(InCommonNew input);
        void UpdateFieldStruct(long updateId, EnumModule module, List<FieldStruct> fieldStructs);

        Task UpdateFieldValue(long IssueId, List<FieldValue> fieldValues);

        Task<OutputDetailIssue> Get([FromQuery] BaseId input);

        /// <summary>
        /// 根据基础条件筛选
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResult<OutputGeneralIssue>> PageWithGeneralCondition([FromQuery] BaseQueryModel input);
        /// <summary>
        /// 基础条件 + 创建者
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResult<OutputGeneralIssue>> PageByCreator([FromQuery] QueryListByCreator input);
        Task<PageResult<OutputGeneralIssue>> PageByDispatcher([FromQuery] QueryListByDispatcher input);
        Task<PageResult<OutputGeneralIssue>> PageByExector([FromQuery] QueryListByExecutor input);
        Task<PageResult<OutputGeneralIssue>> PageBySolved([FromQuery] QueryListInSolved input);
        Task<PageResult<OutputGeneralIssue>> PageByUnSolvd([FromQuery] QueryListInUnSolve input);

        Task<List<OutputGeneralIssue>> List([FromQuery] BaseQueryModel input);
    }
}