﻿using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    public interface ISsuGroupService
    {
        Task Add(AddSsuGroupInput input);

        Task Delete(DeleteSsuGroupInput input);

        Task<SsuGroupOutput> Get([FromQuery] QueryeSsuGroupInput input);

        Task<List<SsuGroupOutput>> Select();

        Task<PageResult<SsuGroupOutput>> Page([FromQuery] SsuGroupInput input);

        Task Update(UpdateSsuGroupInput input);


        Task<PageResult<UserOutput>> GetGroupUsers(SsuGroupUserInput groupInput);

        Task InsertUserGroup(long groupId, IEnumerable<long> userIds);
    }
}