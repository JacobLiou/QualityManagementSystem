using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.IssueService.Dto.Field;
using QMS.Core;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.Linq.Dynamic.Core;

namespace QMS.Application.IssueService
{
    /// <summary>
    /// 问题管理服务
    /// </summary>
    [ApiDescriptionSettings("问题管理", Name = nameof(MyIssue), Order = 100)]
    public class MyIssue : IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssue, IssuesDbContextLocator> _commonIssuesRep;
        private readonly IRepository<SsuIssueDetail, IssuesDbContextLocator> _detailIssuesRep;
        private readonly IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> _fieldStructIssuesRep;
        private readonly IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> _fieldValueIssuesRep;
        private readonly IRepository<SsuIssueOperation, IssuesDbContextLocator> _operationTypeRep;

        public MyIssue(
            IRepository<SsuIssue, IssuesDbContextLocator> commonIssuesRep,
            IRepository<SsuIssueDetail, IssuesDbContextLocator> detailIssuesRep,
            IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> fieldStructIssuesRep,
            IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> fieldValueIssuesRep,
            IRepository<SsuIssueOperation, IssuesDbContextLocator> operationTypeRep
        )
        {
            this._commonIssuesRep = commonIssuesRep;
            this._detailIssuesRep = detailIssuesRep;
            this._fieldStructIssuesRep = fieldStructIssuesRep;
            this._fieldValueIssuesRep = fieldValueIssuesRep;
            this._operationTypeRep = operationTypeRep;

        }

        [HttpPost($"/{nameof(MyIssue)}/update-field-struct")]
        public void UpdateFieldStruct(long updateId, EnumModule module, List<FieldStruct> fieldStructs)
        {
            DateTime now = DateTime.Now;

            var collection = this._fieldStructIssuesRep.DetachedEntities
                .Where(attribute => attribute.Module == module)
                .Where(attribute => fieldStructs.Any(fieldStruct => fieldStruct.FieldCode == attribute.AttributeCode))
                .ToArray();

            Helper.Assert(collection != null && collection.Length > 0, "");

            foreach (var item in collection)
            {
                foreach (var field in fieldStructs)
                {
                    if (field.FieldCode == item.AttributeCode)
                    {
                        item.AttibuteName = field.FieldName;
                    }
                }
            }

            this._fieldStructIssuesRep.Entities.UpdateRange(collection);
            this._fieldStructIssuesRep.Context.SaveChangesAsync();
        }

        [HttpPost($"/{nameof(MyIssue)}/add-field-struct")]
        public async Task AddFieldStruct(long creatorId, EnumModule module, List<FieldStruct> fields)
        {
            DateTime now = DateTime.Now;
            IEnumerable<SsuIssueExtendAttribute> attributes =
                fields.Select<FieldStruct, SsuIssueExtendAttribute>(
                    fieldStruct =>
                        new SsuIssueExtendAttribute()
                        {
                            AttibuteName = fieldStruct.FieldName,
                            Module = module,
                            AttributeCode = fieldStruct.FieldCode,
                            ValueType = fieldStruct.FiledDataType,
                            CreateTime = now,
                            CreatorId = creatorId,
                            UpdateId = creatorId,
                            UpdateTime = now
                        }
            );

            await this._fieldStructIssuesRep.Entities.AddRangeAsync(attributes.ToArray());
            await this._fieldStructIssuesRep.Context.SaveChangesAsync();
        }

        [HttpPost($"/{nameof(MyIssue)}/add-field-value")]
        public async Task AddFieldValue(long IssueId, List<FieldValue> fieldValues)
        {
            DateTime now = DateTime.Now;

            // 找到对应的字段编号
            var array = this._fieldStructIssuesRep.DetachedEntities.Where<SsuIssueExtendAttribute>(field =>
               fieldValues.Any<FieldValue>(value => value.AttributeCode == field.AttributeCode)
           ).ToArray();

            // 根据字段编号和问题Id插入数据
            await this._fieldValueIssuesRep.Entities.AddRangeAsync(array.Select<SsuIssueExtendAttribute, SsuIssueExtendAttributeValue>(attribute =>
                 new SsuIssueExtendAttributeValue()
                 {
                     Id = attribute.Id,
                     IssueNum = IssueId,
                     AttibuteValue = fieldValues.FirstOrDefault(value => value.AttributeCode == attribute.AttributeCode).Value
                 })
             );
        }

        [HttpPost($"/{nameof(MyIssue)}/update-field-value")]
        public async Task UpdateFieldValue(long IssueId, List<FieldValue> fieldValues)
        {
            DateTime now = DateTime.Now;

            // 找到对应的字段编号
            var array = this._fieldStructIssuesRep.DetachedEntities.Where<SsuIssueExtendAttribute>(field =>
               fieldValues.Any<FieldValue>(value => value.AttributeCode == field.AttributeCode)
           ).ToArray();

            Helper.Assert(array != null && array.Length > 0, "字段都不存在");

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

            var values = this._fieldValueIssuesRep.Entities
                .Where<SsuIssueExtendAttributeValue>(
                value =>
                value.IssueNum == IssueId
                && array.Any<SsuIssueExtendAttribute>(attribute => attribute.Id == value.Id)
                );


            foreach (var item in values)
            {
                item.AttibuteValue = dic[item.Id];
            }

            this._fieldValueIssuesRep.Entities.UpdateRange(values);
            await this._fieldValueIssuesRep.Context.SaveChangesAsync();
        }

    }
}
