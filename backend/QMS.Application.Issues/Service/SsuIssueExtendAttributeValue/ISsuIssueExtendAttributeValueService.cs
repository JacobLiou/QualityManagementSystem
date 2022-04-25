using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{
    public interface ISsuIssueExtendAttributeValueService
    {
        Task Add(AddSsuIssueExtendAttributeValueInput input);
        Task Delete(DeleteSsuIssueExtendAttributeValueInput input);
        Task<SsuIssueExtendAttributeValueOutput> Get([FromQuery] QueryeSsuIssueExtendAttributeValueInput input);
        Task<List<SsuIssueExtendAttributeValueOutput>> List([FromQuery] SsuIssueExtendAttributeValueInput input);
        Task<PageResult<SsuIssueExtendAttributeValueOutput>> Page([FromQuery] SsuIssueExtendAttributeValueInput input);
        Task Update(UpdateSsuIssueExtendAttributeValueInput input);
    }
}