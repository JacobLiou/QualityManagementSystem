using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    public interface ISsuProductService
    {
        Task Add(AddSsuProductInput input);
        Task Delete(DeleteSsuProductInput input);
        Task<SsuProductOutput> Get([FromQuery] QueryeSsuProductInput input);
        Task<List<SsuProductOutput>> List([FromQuery] SsuProductInput input);
        Task<PageResult<SsuProductOutput>> Page([FromQuery] SsuProductInput input);
        Task Update(UpdateSsuProductInput input);
    }
}