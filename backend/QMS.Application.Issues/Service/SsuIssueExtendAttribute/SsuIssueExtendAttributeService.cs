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
        /// 分页查询问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueExtendAttribute/page")]
        public async Task<PageResult<SsuIssueExtendAttributeOutput>> Page([FromQuery] SsuIssueExtendAttributeInput input)
        {
            var ssuIssueExtendAttributes = await _ssuIssueExtendAttributeRep.DetachedEntities
                                     .Where(input.Module != null, u => u.Module == input.Module)
                                     .Where(!string.IsNullOrEmpty(input.AttibuteName), u => EF.Functions.Like(u.AttibuteName, $"%{input.AttibuteName.Trim()}%"))
                                     .Where(!string.IsNullOrEmpty(input.AttributeCode), u => u.AttributeCode == input.AttributeCode)
                                     .Where(!string.IsNullOrEmpty(input.ValueType), u => u.ValueType == input.ValueType)
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuIssueExtendAttributeInput>(input))
                                     .ProjectToType<SsuIssueExtendAttributeOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssueExtendAttributes;
        }

        /// <summary>
        /// 增加问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueExtendAttribute/add")]
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
        [HttpPost("/SsuIssueExtendAttribute/delete")]
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
        [HttpPost("/SsuIssueExtendAttribute/edit")]
        public async Task Update(UpdateSsuIssueExtendAttributeInput input)
        {
            var isExist = await _ssuIssueExtendAttributeRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuIssueExtendAttribute = input.Adapt<SsuIssueExtendAttribute>();
            ssuIssueExtendAttribute.SetUpdate();

            await _ssuIssueExtendAttributeRep.UpdateAsync(ssuIssueExtendAttribute, ignoreNullValues: true);

            await IssueLogger.Log(this._ssuIssueOperationRep, Helper.Helper.GetCurrentUser(), Core.Enum.EnumIssueOperationType.Edit, JsonConvert.SerializeObject(input));
        }

        /// <summary>
        /// 获取问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueExtendAttribute/detail")]
        public async Task<SsuIssueExtendAttributeOutput> Get([FromQuery] QueryeSsuIssueExtendAttributeInput input)
        {
            return (await _ssuIssueExtendAttributeRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuIssueExtendAttributeOutput>();
        }

        /// <summary>
        /// 获取问题扩展属性列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueExtendAttribute/list")]
        public async Task<List<SsuIssueExtendAttributeOutput>> List([FromQuery] SsuIssueExtendAttributeInput input)
        {
            return await _ssuIssueExtendAttributeRep.DetachedEntities.ProjectToType<SsuIssueExtendAttributeOutput>().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        public class BatchFieldStruct
        {
            public string Operation { get; set; } = "批量增加字段";
            public List<FieldStruct> List { get; set; }
        }

        [HttpPost($"/SsuIssueExtendAttribute/batch-add-struct")]
        public async Task BatchAddFieldStruct(BatchFieldStruct input)
        {
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

            await IssueLogger.Log(this._ssuIssueOperationRep, updateId, Core.Enum.EnumIssueOperationType.New, JsonConvert.SerializeObject(input));
        }

        [HttpPost($"/SsuIssueExtendAttribute/add-field-value")]
        public async Task AddFieldValue(long IssueId, List<FieldValue> fieldValues)
        {
            DateTime now = DateTime.Now;

            // 找到对应的字段编号
            var array = this._ssuIssueExtendAttributeRep.DetachedEntities.Where<SsuIssueExtendAttribute>(field =>
               fieldValues.Any<FieldValue>(value => value.AttributeCode == field.AttributeCode)
           ).ToArray();

            // 根据字段编号和问题Id插入数据
            await this._ssuIssueExtendAttributeValueRep.Entities.AddRangeAsync(array.Select<SsuIssueExtendAttribute, SsuIssueExtendAttributeValue>(attribute =>
                 new SsuIssueExtendAttributeValue()
                 {
                     Id = attribute.Id,
                     IssueNum = IssueId,
                     AttibuteValue = fieldValues.FirstOrDefault(value => value.AttributeCode == attribute.AttributeCode).Value
                 })
             );
        }

        [HttpPost($"/SsuIssueExtendAttribute/update-field-value")]
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
