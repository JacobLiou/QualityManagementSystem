using Furion.Extras.Admin.NET;
using static QMS.Application.Issues.IssueOperationService;

namespace QMS.Application.Issues
{
    public interface IIssueOperationService
    {
        //Task Add(AddIssueOperationInput input);
        //Task Delete(DeleteIssueOperationInput input);
        //Task<IssueOperationOutput> Get([FromQuery] QueryeIssueOperationInput input);
        //Task<List<IssueOperationOutput>> List([FromQuery] IssueOperationInput input);
        Task<PageResult<IssueOperationOutput>> Page(IssueIdModel input);
        //Task Update(UpdateIssueOperationInput input);
    }
}