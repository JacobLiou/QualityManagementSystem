using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{
    public interface ISsuIssuesService
    {
        Task Add(AddSsuIssuesInput input);
        Task Delete(DeleteSsuIssuesInput input);
        Task<SsuIssuesOutput> Get([FromQuery] QueryeSsuIssuesInput input);
        Task<List<SsuIssuesOutput>> List([FromQuery] SsuIssuesInput input);
        Task<PageResult<SsuIssuesOutput>> Page([FromQuery] SsuIssuesInput input);
        Task Update(UpdateSsuIssuesInput input);
    }
}