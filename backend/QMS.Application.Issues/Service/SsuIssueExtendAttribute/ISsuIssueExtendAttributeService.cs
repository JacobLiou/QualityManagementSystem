using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.Field;
using static QMS.Application.Issues.SsuIssueExtendAttributeService;

namespace QMS.Application.Issues
{
    public interface ISsuIssueExtendAttributeService
    {
        Task Add(AddSsuIssueExtendAttributeInput input);
        Task Delete(DeleteSsuIssueExtendAttributeInput input);
        //Task<SsuIssueExtendAttributeOutput> Get([FromQuery] QueryeSsuIssueExtendAttributeInput input);
        Task<List<FieldStruct>> List([FromQuery] ModuleType input);
        //Task<PageResult<SsuIssueExtendAttributeOutput>> Page([FromQuery] SsuIssueExtendAttributeInput input);
        Task Update(UpdateSsuIssueExtendAttributeInput input);
    }
}