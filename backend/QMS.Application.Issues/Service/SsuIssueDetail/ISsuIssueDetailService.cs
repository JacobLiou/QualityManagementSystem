using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.IssueService.Dto.QueryList;

namespace QMS.Application.Issues
{
    public interface ISsuIssueDetailService
    {
        Task Add(AddSsuIssueDetailInput input);
        Task Delete(DeleteSsuIssueDetailInput input);
        Task<SsuIssueDetailOutput> Get([FromQuery] QueryeSsuIssueDetailInput input);
        Task<List<SsuIssueDetailOutput>> List([FromQuery] SsuIssueDetailInput input);
        Task<PageResult<SsuIssueDetailOutput>> Page([FromQuery] SsuIssueDetailInput input);
        Task Update(UpdateSsuIssueDetailInput input);
    }
}