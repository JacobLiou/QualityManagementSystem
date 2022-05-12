using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.Issues
{
    public interface IIssueOperationService
    {
        //Task Add(AddIssueOperationInput input);
        //Task Delete(DeleteIssueOperationInput input);
        //Task<IssueOperationOutput> Get([FromQuery] QueryeIssueOperationInput input);
        //Task<List<IssueOperationOutput>> List([FromQuery] IssueOperationInput input);
        Task<PageResult<IssueOperationOutput>> Page([FromQuery] IssueOperationInput input);
        //Task Update(UpdateIssueOperationInput input);
    }
}