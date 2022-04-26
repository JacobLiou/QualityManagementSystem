using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    public interface ISsuProjectService
    {
        Task Add(AddSsuProjectInput input);
        Task Delete(DeleteSsuProjectInput input);
        Task<SsuProjectOutput> Get([FromQuery] QueryeSsuProjectInput input);
        Task<List<SsuProjectOutput>> List([FromQuery] SsuProjectInput input);
        Task<PageResult<SsuProjectOutput>> Page([FromQuery] SsuProjectInput input);
        Task Update(UpdateSsuProjectInput input);
    }
}