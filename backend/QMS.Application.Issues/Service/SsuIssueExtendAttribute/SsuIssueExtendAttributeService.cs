using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
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
    [ApiDescriptionSettings("问题管理服务", Name = "IssueExtAttr", Order = 100)]
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
        [HttpPost("/issue/extAttr/addStruct")]
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
        [HttpPost("/issue/extAttr/deleteStruct")]
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
        [HttpPost("/issue/extAttr/editStruct")]
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

        /// <summary>
        /// 分页查询问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/issue/extAttr/page")]
        public async Task<PageResult<SsuIssueExtendAttributeOutput>> Page([FromQuery] SsuIssueExtendAttributeInput input)
        {
            var ssuIssueExtendAttributes = await _ssuIssueExtendAttributeRep.DetachedEntities
                                     .Where(input.Module != null, u => u.Module == input.Module)
                                     .Where(!string.IsNullOrEmpty(input.AttibuteName), u => u.AttibuteName == input.AttibuteName)
                                     .Where(!string.IsNullOrEmpty(input.AttributeCode), u => u.AttributeCode == input.AttributeCode)
                                     .Where(!string.IsNullOrEmpty(input.ValueType), u => u.ValueType == input.ValueType)
                                     //.Where(u => u.CreatorId == input.CreatorId)
                                     //.Where(u => u.CreateTime == input.CreateTime)
                                     //.Where(u => u.UpdateId == input.UpdateId)
                                     //.Where(u => u.UpdateTime == input.UpdateTime)
                                     //.Where(u => u.Sort == input.Sort)
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuIssueExtendAttributeInput>(input))
                                     .ProjectToType<SsuIssueExtendAttributeOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssueExtendAttributes;
        }

        public class ModuleType
        {
            public EnumModule Module { get; set; }
        }

        /// <summary>
        /// 根据模块获取相应扩展字段结构集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/issue/extAttr/listStruct")]
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

        [HttpPost($"/issue/extAttr/batchAddStruct")]
        public async Task BatchAddFieldStruct(List<FieldStruct> input)
        {
            if (input == null || input.Count == 0)
            {
                throw Oops.Oh(ErrorCode.D1007);
            }

            var list = this._ssuIssueExtendAttributeRep.DetachedEntities
                .Where<SsuIssueExtendAttribute>(attr => attr.Module == input.First().Module)
                .Select<SsuIssueExtendAttribute, string>(attr => attr.AttributeCode);


            var finalyList = input.Where<FieldStruct>(field => !list.Contains(field.FieldCode));


            //IEqualityComparer<string> equalityComparer = new MyStringComparer();

            //Helper.Helper.Assert(!this._ssuIssueExtendAttributeRep
            //    .Any(
            //        model =>
            //        input.Select<FieldStruct, string>(field => field.FieldCode)
            //        .Contains(model.AttributeCode, equalityComparer)
            //    ),
            //    "同名属性编码已存在"
            //    );

            long updateId = Helper.Helper.GetCurrentUser();
            DateTime now = DateTime.Now;
            IEnumerable<SsuIssueExtendAttribute> attributes =
                finalyList.Select<FieldStruct, SsuIssueExtendAttribute>(
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


        [HttpGet("/issue/extAttr/template")]
        public async Task<IActionResult> Template()
        {
            var item = this._ssuIssueExtendAttributeRep.Where(model => 1 == 0).Take(1).ProjectToType<AddSsuIssueExtendAttributeInput>();
            return await Helper.Helper.ExportExcel(item, "IssueExtAttrTemplate");
        }

        /// <summary>
        /// 问题数据导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/issue/extAttr/import")]
        public async Task ImportIssues(IFormFile file)
        {
            Helper.Helper.Assert(file != null && !string.IsNullOrEmpty(file.FileName), "文件为空");

            Helper.Helper.Assert(file.FileName, fileName => fileName.Contains("IssueExtAttrTemplate") && fileName.EndsWith(".xlsx"), "请使用下载的模板进行数据导入");

            IEnumerable<dynamic> collection = MiniExcel.Query(file.OpenReadStream(), true);

            foreach (var item in collection)
            {
                var extendAttr = new AddSsuIssueExtendAttributeInput()
                {
                    Module = (EnumModule)Helper.Helper.GetIntFromEnumDescription(item.模块名),
                    AttibuteName = item.字段名,
                    AttributeCode = item.字段代码,
                    ValueType = item.字段值类型,
                };

                await this.Add(extendAttr);
            }
        }


        public class BatchFieldValue
        {
            public long IssueId { get; set; }
            public List<FieldValue> List { get; set; }
        }

        [Obsolete]
        [HttpPost($"/issue/extAttr/batchAddValue")]
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
        [HttpPost($"/issue/extAttr/updateValue")]
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
