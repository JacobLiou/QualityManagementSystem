using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Entity.Common;
using Furion.Extras.Admin.NET.Service;
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

        Task<List<SsuProjectOutput>> Select();

        Task<PageResult<SsuProjectOutput>> Page([FromQuery] SsuProjectInput input);

        Task Update(UpdateSsuProjectInput input);

        Task<PageResult<UserOutput>> GetProjectUser(SsuProjectUserInput projectInput);

        Task InsertProjectGroup(long projectId, IEnumerable<long> userIds);

        Task<List<SsuProject>> GetProjectList(IEnumerable<UpdateSsuProjectInput> projectInput);
    }
}