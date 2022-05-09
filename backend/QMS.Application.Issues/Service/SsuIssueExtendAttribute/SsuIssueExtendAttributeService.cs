using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QMS.Application.Issues.Field;
using QMS.Application.Issues.Helper;
using QMS.Core;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.Linq.Dynamic.Core;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题扩展属性服务
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "SsuIssueExtendAttribute", Order = 100)]
    public class SsuIssueExtendAttributeService : ISsuIssueExtendAttributeService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> _ssuIssueExtendAttributeRep;
        private readonly IRepository<SsuIssueOperation, IssuesDbContextLocator> _ssuIssueOperationRep;
        private readonly IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> _ssuIssueExtendAttributeValueRep;

        public SsuIssueExtendAttributeService(
            IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> ssuIssueExtendAttributeRep,
            IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> ssuIssueExtendAttributeValueRep,
            IRepository<SsuIssueOperation, IssuesDbContextLocator> ssuIssueOperationRep
        )
        {
            _ssuIssueExtendAttributeRep = ssuIssueExtendAttributeRep;
            _ssuIssueExtendAttributeValueRep = ssuIssueExtendAttributeValueRep;
            _ssuIssueOperationRep = ssuIssueOperationRep;
        }

        /// <summary>
        /// 增加问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueExtendAttribute/AddStruct")]
        public async Task Add(AddSsuIssueExtendAttributeInput input)
        {
            var ssuIssueExtendAttribute = input.Adapt<SsuIssueExtendAttribute>();
            ssuIssueExtendAttribute.SetCreate();

            await _ssuIssueExtendAttributeRep.InsertAsync(ssuIssueExtendAttribute);

            await IssueLogger.Log(this._ssuIssueOperationRep, Helper.Helper.GetCurrentUser(), Core.Enum.EnumIssueOperationType.New, JsonConvert.SerializeObject(input));
        }

        /// <summary>
        /// 删除问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueExtendAttribute/DeleteStruct")]
        public async Task Delete(DeleteSsuIssueExtendAttributeInput input)
        {
            var ssuIssueExtendAttribute = await _ssuIssueExtendAttributeRep.FirstOrDefaultAsync(u => u.Id == input.Id);

            if (ssuIssueExtendAttribute != null && !ssuIssueExtendAttribute.IsDeleted)
            {
                ssuIssueExtendAttribute.SetDelete();

                await _ssuIssueExtendAttributeRep.UpdateAsync(ssuIssueExtendAttribute);

                await IssueLogger.Log(this._ssuIssueOperationRep, Helper.Helper.GetCurrentUser(), Core.Enum.EnumIssueOperationType.Edit, JsonConvert.SerializeObject(input));
            }
        }

        /// <summary>
        /// 更新问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueExtendAttribute/EditStruct")]
        public async Task Update(UpdateSsuIssueExtendAttributeInput input)
        {
            var isExist = await _ssuIssueExtendAttributeRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D1007);

            var ssuIssueExtendAttribute = input.Adapt<SsuIssueExtendAttribute>();
            ssuIssueExtendAttribute.SetUpdate();

            await _ssuIssueExtendAttributeRep.UpdateAsync(ssuIssueExtendAttribute, ignoreNullValues: true);

            await IssueLogger.Log(this._ssuIssueOperationRep, Helper.Helper.GetCurrentUser(), Core.Enum.EnumIssueOperationType.Edit, JsonConvert.SerializeObject(input));
        }

        ///// <summary>
        ///// 获取问题扩展属性
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpGet("/SsuIssueExtendAttribute/detail")]
        //public async Task<SsuIssueExtendAttributeOutput> Get([FromQuery] QueryeSsuIssueExtendAttributeInput input)
        //{
        //    return (await _ssuIssueExtendAttributeRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuIssueExtendAttributeOutput>();
        //}

        public class ModuleType
        {
            public EnumModule Module { get; set; }
        }

        /// <summary>
        /// 根据模块获取相应扩展字段结构集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueExtendAttribute/ListStruct")]
        public async Task<List<FieldStruct>> List([FromQuery] ModuleType input)
        {
            return await _ssuIssueExtendAttributeRep.DetachedEntities
                .Where<SsuIssueExtendAttribute>(attr => attr.Module == input.Module)
                .Select<SsuIssueExtendAttribute, FieldStruct>(extend => new FieldStruct
                {
                    Module = extend.Module,
                    FieldCode = extend.AttributeCode,
                    FieldName = extend.AttibuteName,
                    FiledDataType = extend.ValueType,
                    FieldId = extend.Id
                }).ToListAsync();
        }

        public class BatchFieldStruct
        {
            public List<FieldStruct> List { get; set; }
        }

        [HttpPost($"/SsuIssueExtendAttribute/BatchAddStruct")]
        public async Task BatchAddFieldStruct(BatchFieldStruct input)
        {
            if (input.List==null || input.List.Count == 0)
            {
                throw Oops.Oh(ErrorCode.D1007);
            }

            long updateId = Helper.Helper.GetCurrentUser();
            DateTime now = DateTime.Now;
            IEnumerable<SsuIssueExtendAttribute> attributes =
                input.List.Select<FieldStruct, SsuIssueExtendAttribute>(
                    fieldStruct =>
                        new SsuIssueExtendAttribute()
                        {
                            AttibuteName = fieldStruct.FieldName,
                            Module = fieldStruct.Module,
                            AttributeCode = fieldStruct.FieldCode,
                            ValueType = fieldStruct.FiledDataType,
                            CreateTime = now,
                            CreatorId = updateId,
                            UpdateId = updateId,
                            UpdateTime = now
                        }
            );

            await this._ssuIssueExtendAttributeRep.Entities.AddRangeAsync(attributes.ToArray());
            await this._ssuIssueExtendAttributeRep.Context.SaveChangesAsync();

            await IssueLogger.Log(this._ssuIssueOperationRep, updateId, EnumIssueOperationType.New, JsonConvert.SerializeObject(input));
        }

        public class BatchFieldValue
        {
            public long IssueId { get; set; }
            public List<FieldValue> List { get; set; }
        }

        [Obsolete]
        [HttpPost($"/SsuIssueExtendAttribute/BatchAddValue")]
        public async Task AddFieldValue(BatchFieldValue input)
        {
            DateTime now = DateTime.Now;

            // 根据字段编号和问题Id插入数据
            await this._ssuIssueExtendAttributeValueRep.Entities.AddRangeAsync(input.List.Select<FieldValue, SsuIssueExtendAttributeValue>(attribute =>
                 new SsuIssueExtendAttributeValue()
                 {
                     Id = attribute.AttributeId,
                     IssueNum = input.IssueId,
                     AttibuteValue = attribute.Value
                 })
             );
        }

        [Obsolete]
        [HttpPost($"/SsuIssueExtendAttribute/UpdateValue")]
        public async Task UpdateFieldValue(long IssueId, List<FieldValue> fieldValues)
        {
            DateTime now = DateTime.Now;

            // 找到对应的字段编号
            var array = this._ssuIssueExtendAttributeRep.DetachedEntities.Where<SsuIssueExtendAttribute>(field =>
               fieldValues.Any<FieldValue>(value => value.AttributeCode == field.AttributeCode)
           ).ToArray();

            Helper.Helper.Assert(array != null && array.Length > 0, "字段都不存在");

            // 收集字段Id和字段值的关系
            Dictionary<long, string> dic = new Dictionary<long, string>();
            foreach (var item in array)
            {
                foreach (var field in fieldValues)
                {
                    if (field.AttributeCode == item.AttributeCode)
                    {
                        dic.Add(item.Id, field.Value);
                    }
                }
            }

            var values = this._ssuIssueExtendAttributeValueRep.Entities.Where<SsuIssueExtendAttributeValue>(
                value =>
                value.IssueNum == IssueId
                && array.Any<SsuIssueExtendAttribute>(attribute => attribute.Id == value.Id)
                );


            foreach (var item in values)
            {
                item.AttibuteValue = dic[item.Id];
            }

            this._ssuIssueExtendAttributeValueRep.Entities.UpdateRange(values);
            await this._ssuIssueExtendAttributeValueRep.Context.SaveChangesAsync();
        }
    }
}
