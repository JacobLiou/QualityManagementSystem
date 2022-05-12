using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.Field;
using static QMS.Application.Issues.IssueExtendAttributeService;

namespace QMS.Application.Issues
{
    public interface IIssueExtendAttributeService
    {
        Task Add(AddIssueExtendAttributeInput input);
        Task Delete(DeleteIssueExtendAttributeInput input);
        //Task<IssueExtendAttributeOutput> Get([FromQuery] QueryeIssueExtendAttributeInput input);
        Task<List<FieldStruct>> List([FromQuery] MoudleModel input);
        //Task<PageResult<IssueExtendAttributeOutput>> Page([FromQuery] IssueExtendAttributeInput input);
        Task Update(UpdateIssueExtendAttributeInput input);
    }
}