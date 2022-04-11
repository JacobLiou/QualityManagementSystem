using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysDictDataService
    {
        Task AddDictData(AddDictDataInput input);

        Task ChangeDictDataStatus(ChageStateDictDataInput input);

        Task DeleteByTypeId(long dictTypeId);

        Task DeleteDictData(DeleteDictDataInput input);

        Task<SysDictData> GetDictData([FromQuery] QueryDictDataInput input);

        Task<List<SysDictData>> GetDictDataList([FromQuery] QueryDictDataListInput input);

        Task<List<SysDictData>> GetDictDataListByDictTypeId(long dictTypeId);

        Task<PageResult<DictDataOutput>> QueryDictDataPageList([FromQuery] DictDataPageInput input);

        Task UpdateDictData(UpdateDictDataInput input);
    }
}