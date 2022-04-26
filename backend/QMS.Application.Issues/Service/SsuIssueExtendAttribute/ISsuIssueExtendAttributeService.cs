using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{
    public interface ISsuIssueExtendAttributeService
    {
        Task Add(AddSsuIssueExtendAttributeInput input);
        Task Delete(DeleteSsuIssueExtendAttributeInput input);
        Task<SsuIssueExtendAttributeOutput> Get([FromQuery] QueryeSsuIssueExtendAttributeInput input);
        Task<List<SsuIssueExtendAttributeOutput>> List([FromQuery] SsuIssueExtendAttributeInput input);
        Task<PageResult<SsuIssueExtendAttributeOutput>> Page([FromQuery] SsuIssueExtendAttributeInput input);
        Task Update(UpdateSsuIssueExtendAttributeInput input);
    }
}