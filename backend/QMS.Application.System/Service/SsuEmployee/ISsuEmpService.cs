using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    public interface ISsuEmpService
    {
        Task<List<long>> GetDataScopeListByDataScopeType(int dataScopeType, long orgId);

        Task<SysOrg> GetOrg([FromQuery] QueryOrgInput input);

        Task<List<OrgOutput>> GetOrgList([FromQuery] OrgListInput input);

        Task<dynamic> GetOrgTree();

        Task<List<long>> GetAllDataScopeIdList();

        Task<List<long>> GetUserDataScopeIdList();

        Task<List<UserOutput>> GetUserList(long[] userIds);
    }
}