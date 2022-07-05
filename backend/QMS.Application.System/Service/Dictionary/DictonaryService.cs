using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using QMS.Core;

namespace QMS.Application.System
{
    /// <summary>
    /// 数据库字典数据服务
    /// </summary>
    [ApiDescriptionSettings(Name = "DictonaryService", Order = 100)]
    public class DictonaryService : IDynamicApiController, ITransient
    {
        private readonly IRepository<SysDictType> _sysDictType;
        private readonly IRepository<SysDictData> _sysDictData;
        private readonly ICacheService _cacheService;

        public DictonaryService(IRepository<SysDictType> sysDictType, IRepository<SysDictData> sysDictData, ICacheService cacheService)
        {
            _sysDictType = sysDictType;
            _sysDictData = sysDictData;
            _cacheService = cacheService;
        }

        /// <summary>
        /// 根据code值获取type类型
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<DictTypeModel> GetTypeDetail(string code)
        {
            var dictType = _sysDictType.DetachedEntities.FirstOrDefault(u => u.Code == code && u.IsDeleted == false);
            if (dictType == null)
            {
                return null;
            }
            return dictType.Adapt<DictTypeModel>();
        }

        /// <summary>
        /// 详情value获取明细
        /// </summary>
        /// <param name="valueInput">sys_dict_data对象</param>
        /// <returns></returns>
        [HttpPost("/dictonaryservice/getdictdetail")]
        public async Task<List<DictDataModel>> GetDictDetail(IEnumerable<DictDataPageInput> valueInput)
        {
            List<DictDataModel> list = new List<DictDataModel>();
            //先从缓存中取值
            foreach (DictDataPageInput value in valueInput)
            {
                var cacheDictData = await _cacheService.GetCache<DictDataModel>(CoreCommonConst.MODULARVALUE + value.Value);
                if (cacheDictData != null)
                {
                    list.Add(cacheDictData);
                }
            }

            //缓存中不存在值则从数据库中获取
            var otherInput = valueInput.Where(u => !list.Select(t => t.Value).Contains(u.Value)).Select(u => u.Value);
            var dictData = _sysDictData.DetachedEntities.Where(u => otherInput.Contains(u.Value)).Select(u => u.Adapt<DictDataModel>());
            foreach (DictDataModel obj in dictData)
            {
                list.Add(obj);
                await _cacheService.SetCacheByHours(CoreCommonConst.MODULARVALUE + obj.Value, obj, 12);
            }
            return list;
        }
    }
}