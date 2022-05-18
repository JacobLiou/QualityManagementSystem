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
    public class IssueExtendAttributeService : IIssueExtendAttributeService, IDynamicApiController, ITransient
    {
        private readonly IRepository<IssueExtendAttribute, IssuesDbContextLocator> _issueExtendAttributeRep;
        private readonly IRepository<IssueOperation, IssuesDbContextLocator> _issueOperationRep;
        private readonly IRepository<IssueExtendAttributeValue, IssuesDbContextLocator> _issueExtendAttributeValueRep;

        public IssueExtendAttributeService(
            IRepository<IssueExtendAttribute, IssuesDbContextLocator> issueExtendAttributeRep,
            IRepository<IssueExtendAttributeValue, IssuesDbContextLocator> issueExtendAttributeValueRep,
            IRepository<IssueOperation, IssuesDbContextLocator> issueOperationRep
        )
        {
            _issueExtendAttributeRep = issueExtendAttributeRep;
            _issueExtendAttributeValueRep = issueExtendAttributeValueRep;
            _issueOperationRep = issueOperationRep;
        }

        /// <summary>
        /// 增加问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/extAttr/addStruct")]
        public async Task Add(AddIssueExtendAttributeInput input)
        {
            var issueExtendAttribute = input.Adapt<IssueExtendAttribute>();
            issueExtendAttribute.SetCreate();

            await _issueExtendAttributeRep.InsertAsync(issueExtendAttribute);

            await IssueLogger.Log(this._issueOperationRep, Helper.Helper.GetCurrentUser(), EnumIssueOperationType.New, JsonConvert.SerializeObject(input));
        }

        /// <summary>
        /// 删除问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/extAttr/deleteStruct")]
        public async Task Delete(DeleteIssueExtendAttributeInput input)
        {
            var issueExtendAttribute = await _issueExtendAttributeRep.FirstOrDefaultAsync(u => u.Id == input.Id);

            if (issueExtendAttribute != null && !issueExtendAttribute.IsDeleted)
            {
                issueExtendAttribute.SetDelete();

                await _issueExtendAttributeRep.UpdateAsync(issueExtendAttribute);

                await IssueLogger.Log(this._issueOperationRep, Helper.Helper.GetCurrentUser(), EnumIssueOperationType.Edit, JsonConvert.SerializeObject(input));
            }
        }

        /// <summary>
        /// 更新问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/issue/extAttr/editStruct")]
        public async Task Update(UpdateIssueExtendAttributeInput input)
        {
            var isExist = await _issueExtendAttributeRep.AnyAsync(u => u.Id == input.Id, false);

            Helper.Helper.Assert(isExist, Oops.Oh(ErrorCode.D1007));

            var issueExtendAttribute = input.Adapt<IssueExtendAttribute>();
            issueExtendAttribute.SetUpdate();

            await _issueExtendAttributeRep.UpdateAsync(issueExtendAttribute, ignoreNullValues: true);

            await IssueLogger.Log(this._issueOperationRep, Helper.Helper.GetCurrentUser(), EnumIssueOperationType.Edit, JsonConvert.SerializeObject(input));
        }

        ///// <summary>
        ///// 获取问题扩展属性
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpGet("/IssueExtendAttribute/detail")]
        //public async Task<IssueExtendAttributeOutput> Get([FromQuery] QueryeIssueExtendAttributeInput input)
        //{
        //    return (await _issueExtendAttributeRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<IssueExtendAttributeOutput>();
        //}

        /// <summary>
        /// 分页查询问题扩展属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/issue/extAttr/page")]
        public async Task<PageResult<IssueExtendAttributeOutput>> Page([FromQuery] IssueExtendAttributeInput input)
        {
            var issueExtendAttributes = await _issueExtendAttributeRep.DetachedEntities
                                     .Where(input.Module != null, u => u.Module == input.Module)
                                     .Where(!string.IsNullOrEmpty(input.AttibuteName), u => u.AttibuteName == input.AttibuteName)
                                     .Where(!string.IsNullOrEmpty(input.AttributeCode), u => u.AttributeCode == input.AttributeCode)
                                     .Where(!string.IsNullOrEmpty(input.ValueType), u => u.ValueType == input.ValueType)
                                     //.Where(u => u.CreatorId == input.CreatorId)
                                     //.Where(u => u.CreateTime == input.CreateTime)
                                     //.Where(u => u.UpdateId == input.UpdateId)
                                     //.Where(u => u.UpdateTime == input.UpdateTime)
                                     //.Where(u => u.Sort == input.Sort)
                                     .OrderBy(PageInputOrder.OrderBuilder(input))
                                     .ProjectToType<IssueExtendAttributeOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return issueExtendAttributes;
        }

        public class MoudleModel
        {
            /// <summary>
            /// 模块
            /// </summary>
            public EnumModule Module { get; set; }
        }

        /// <summary>
        /// 根据模块获取相应扩展字段结构集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/issue/extAttr/listStruct")]
        public async Task<List<FieldStruct>> List([FromQuery] MoudleModel input)
        {
            Helper.Helper.Assert(input != null, Oops.Oh(ErrorCode.xg1002));

            return await _issueExtendAttributeRep.DetachedEntities
                .Where(attr => attr.Module == input.Module)
                .Select(extend => new FieldStruct
                {
                    Module = extend.Module,
                    FieldCode = extend.AttributeCode,
                    FieldName = extend.AttibuteName,
                    FieldDataType = extend.ValueType,
                    FieldId = extend.Id
                }).ToListAsync();
        }

        /// <summary>
        /// 批量字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost($"/issue/extAttr/batchAddStruct")]
        public async Task BatchAddFieldStruct(List<FieldStruct> input)
        {
            Helper.Helper.Assert(input != null && input.Count > 0, Oops.Oh(ErrorCode.xg1002));

            var list = this._issueExtendAttributeRep.DetachedEntities
                .Where(attr => attr.Module == input.First().Module)
                .Select(attr => attr.AttributeCode);


            var finalyList = input.Where(field => !list.Contains(field.FieldCode));

            if (finalyList.Any())
            {
                //IEqualityComparer<string> equalityComparer = new MyStringComparer();

                //Helper.Helper.Assert(!this._issueExtendAttributeRep
                //    .Any(
                //        model =>
                //        input.Select<FieldStruct, string>(field => field.FieldCode)
                //        .Contains(model.AttributeCode, equalityComparer)
                //    ),
                //    "同名属性编码已存在"
                //    );

                long updateId = Helper.Helper.GetCurrentUser();
                DateTime now = DateTime.Now;
                IEnumerable<IssueExtendAttribute> attributes =
                    finalyList.Select(
                        fieldStruct =>
                            new IssueExtendAttribute()
                            {
                                AttibuteName = fieldStruct.FieldName,
                                Module = fieldStruct.Module,
                                AttributeCode = fieldStruct.FieldCode,
                                ValueType = fieldStruct.FieldDataType,
                                CreateTime = now,
                                CreatorId = updateId,
                                UpdateId = updateId,
                                UpdateTime = now
                            }
                );

                await this._issueExtendAttributeRep.Entities.AddRangeAsync(attributes.ToArray());
                await this._issueExtendAttributeRep.Context.SaveChangesAsync();

                await IssueLogger.Log(this._issueOperationRep, updateId, EnumIssueOperationType.New, JsonConvert.SerializeObject(input));
            }
        }


        [HttpGet("/issue/extAttr/template")]
        public async Task<IActionResult> Template()
        {
            var item = this._issueExtendAttributeRep.Where(model => true).Take(1).ProjectToType<AddIssueExtendAttributeInput>();
            return await Helper.Helper.ExportExcel(item, "IssueExtAttrTemplate");
        }

        /// <summary>
        /// 问题数据导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("/issue/extAttr/import")]
        public async Task ImportExtAttr(IFormFile file)
        {
            Helper.Helper.Assert(file != null && !string.IsNullOrEmpty(file.FileName), Oops.Oh(ErrorCode.xg1002));

            Helper.Helper.Assert(file.FileName, fileName => fileName.Contains("IssueExtAttrTemplate") && fileName.EndsWith(".xlsx"), "请使用下载的模板进行数据导入");

            IEnumerable<dynamic> collection = 
                MiniExcel.Query(file.OpenReadStream(), true)
                .TakeWhile(item => item.模块名 != null && item.字段代码 != null && item.字段名 != null && item.字段值类型 != null);

            List<string> codeList = await this._issueExtendAttributeRep.DetachedEntities
                .Select(attr => attr.AttributeCode)
                .ToListAsync();

            List<FieldStruct> list = 
                collection.Where(model => !codeList.Contains(model.字段代码))
                .Select(item => new FieldStruct()
                {
                    Module = (EnumModule)Helper.Helper.GetIntFromEnumDescription(item.模块名),
                    FieldCode = item.字段代码,
                    FieldName = item.字段名,
                    FieldDataType = item.字段值类型
                }).ToList();

            await this.BatchAddFieldStruct(list);

            //foreach (var item in collection)
            //{
            //    if (!codeList.Contains(item.字段代码))
            //    {
            //        var extendAttr = new AddIssueExtendAttributeInput()
            //        {
            //            Module = (EnumModule)Helper.Helper.GetIntFromEnumDescription(item.模块名),
            //            AttibuteName = item.字段名,
            //            AttributeCode = item.字段代码,
            //            ValueType = item.字段值类型,
            //        };

            //        //await this.Add(extendAttr);
            //    }
            //}
        }


        public class BatchFieldValue
        {
            /// <summary>
            /// 问题编号
            /// </summary>
            public long IssueId { get; set; }
            /// <summary>
            /// 字段值列表
            /// </summary>
            public List<FieldValue> List { get; set; }
        }

        [Obsolete]
        [HttpPost($"/issue/extAttr/batchAddValue")]
        public async Task AddFieldValue(BatchFieldValue input)
        {
            DateTime now = DateTime.Now;

            // 根据字段编号和问题Id插入数据
            await this._issueExtendAttributeValueRep.Entities.AddRangeAsync(input.List.Select(attribute =>
                 new IssueExtendAttributeValue()
                 {
                     Id = attribute.FieldId,
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
            var array = this._issueExtendAttributeRep.DetachedEntities.Where(field =>
               fieldValues.Any(value => value.FieldCode == field.AttributeCode)
           ).ToArray();

            Helper.Helper.Assert(array != null && array.Length > 0, "字段都不存在");

            // 收集字段Id和字段值的关系
            Dictionary<long, string> dic = new Dictionary<long, string>();
            foreach (var item in array)
            {
                foreach (var field in fieldValues)
                {
                    if (field.FieldCode == item.AttributeCode)
                    {
                        dic.Add(item.Id, field.Value);
                    }
                }
            }

            var values = this._issueExtendAttributeValueRep.Entities.Where(
                value =>
                value.IssueNum == IssueId
                && array.Any(attribute => attribute.Id == value.Id)
                );


            foreach (var item in values)
            {
                item.AttibuteValue = dic[item.Id];
            }

            this._issueExtendAttributeValueRep.Entities.UpdateRange(values);
            await this._issueExtendAttributeValueRep.Context.SaveChangesAsync();
        }
    }
}
