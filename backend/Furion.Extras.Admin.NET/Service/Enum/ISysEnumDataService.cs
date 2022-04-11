using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysEnumDataService
    {
        Task<dynamic> GetEnumDataList([FromQuery] EnumDataInput input);

        Task<dynamic> GetEnumDataListByField([FromQuery] QueryEnumDataInput input);
    }
}