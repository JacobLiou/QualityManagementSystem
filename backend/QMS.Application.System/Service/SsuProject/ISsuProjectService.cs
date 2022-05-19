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

        Task<List<SsuProjectOutput>> List();

        Task<PageResult<SsuProjectOutput>> Page([FromQuery] SsuProjectInput input);

        Task Update(UpdateSsuProjectInput input);

        Task<List<UserOutput>> GetProjectUser(long projectId);

        Task InsertProjectGroup(long projectId, IEnumerable<long> userIds);

        Task<Dictionary<long, SsuProject>> GetProjectList(IEnumerable<long> projectIds);
    }
}