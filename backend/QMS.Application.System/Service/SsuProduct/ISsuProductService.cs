using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Entity.Common;
using Furion.Extras.Admin.NET.Service;
using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.System
{
    public interface ISsuProductService
    {
        Task Add(AddSsuProductInput input);

        Task Delete(DeleteSsuProductInput input);

        Task<SsuProductOutput> Get([FromQuery] QueryeSsuProductInput input);

        Task<List<SsuProductOutput>> List();

        Task<PageResult<SsuProductOutput>> Page([FromQuery] SsuProductInput input);

        Task Update(UpdateSsuProductInput input);

        Task<List<UserOutput>> GetProductUsers(long productId);

        Task InsertProductGroup(long productId, long[] userIds);

        Task<Dictionary<long, SsuProduct>> GetProductList(IEnumerable<long> productIds);
    }
}