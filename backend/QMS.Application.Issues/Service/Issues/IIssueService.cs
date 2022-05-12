using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.Service.Issue.Dto.Add;
using QMS.Application.Issues.Service.Issue.Dto.Query;
using QMS.Application.Issues.Service.Issue.Dto.Update;
using static QMS.Application.Issues.IssueService;

namespace QMS.Application.Issues
{
    public interface IIssueService
    {
        Task<long> Add(InIssue input);
        Task Delete(DeleteIssueInput input);
        Task<OutputDetailIssue> Get([FromQuery] BaseId input);
        //Task<List<IssueOutput>> List([FromQuery] IssueInput input);
        //Task<PageResult<IssueOutput>> Page([FromQuery] IssueInput input);
        Task Edit(UpdateIssueInput input);

        Task Execute(InSolve input);
        Task Validate(InValidate input);

        Task HangUp(InHangup input);

        Task ReDispatch(InReDispatch input);

        Task Dispatch(InDispatch input);

        /// <summary>
        /// 根据基础条件筛选
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResult<OutputGeneralIssue>> Page([FromQuery] BaseQueryModel input);

        Task<IActionResult> Export([FromQuery] BaseQueryModel input);
    }
}