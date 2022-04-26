using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.IssueService.Dto.QueryList;
using QMS.Application.Issues.Service.SsuIssue.Dto.Add;
using QMS.Application.Issues.Service.SsuIssue.Dto.Update;

namespace QMS.Application.Issues
{
    public interface ISsuIssueService
    {
        Task Add(InIssue input);
        Task Delete(DeleteSsuIssueInput input);
        Task<OutputDetailIssue> Get([FromQuery] BaseId input);
        Task<List<SsuIssueOutput>> List([FromQuery] SsuIssueInput input);
        //Task<PageResult<SsuIssueOutput>> Page([FromQuery] SsuIssueInput input);
        Task Update(UpdateSsuIssueInput input);

        Task Update(InSolve input);
        Task Update(InValidate input);

        Task Update(InHangup input);

        Task Update(InReDispatch input);

        /// <summary>
        /// 根据基础条件筛选
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResult<OutputGeneralIssue>> Page([FromQuery] BaseQueryModel input);

        Task Export([FromQuery] BaseQueryModel input);
    }
}