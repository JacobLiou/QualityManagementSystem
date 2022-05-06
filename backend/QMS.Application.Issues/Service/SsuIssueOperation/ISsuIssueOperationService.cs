using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.Issues
{
    public interface ISsuIssueOperationService
    {
        //Task Add(AddSsuIssueOperationInput input);
        //Task Delete(DeleteSsuIssueOperationInput input);
        //Task<SsuIssueOperationOutput> Get([FromQuery] QueryeSsuIssueOperationInput input);
        //Task<List<SsuIssueOperationOutput>> List([FromQuery] SsuIssueOperationInput input);
        Task<PageResult<SsuIssueOperationOutput>> Page([FromQuery] SsuIssueOperationInput input);
        //Task Update(UpdateSsuIssueOperationInput input);
    }
}