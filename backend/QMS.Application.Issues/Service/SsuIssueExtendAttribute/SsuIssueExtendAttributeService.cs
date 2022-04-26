using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [ApiDescriptionSettings("自己的业务", Name = "SsuIssueExtendAttribute", Order = 100)]
    public class SsuIssueExtendAttributeService : ISsuIssueExtendAttributeService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> _ssuIssueExtendAttributeRep;

        private readonly IRepository<SsuIssueOperation, IssuesDbContextLocator> _ssuIssueOperateRep;

        public SsuIssueExtendAttributeService(
            IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> ssuIssueExtendAttributeRep,
            IRepository<SsuIssueOperation, IssuesDbContextLocator> ssuIssueOperateRep
        )
        {
            _ssuIssueExtendAttributeRep = ssuIssueExtendAttributeRep;
            _ssuIssueOperateRep = ssuIssueOperateRep;
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
                                     .Where(u => u.Module == input.Module)
                                     .Where(!string.IsNullOrEmpty(input.AttibuteName), u => u.AttibuteName == input.AttibuteName)
                                     .Where(!string.IsNullOrEmpty(input.AttributeCode), u => u.AttributeCode == input.AttributeCode)
                                     .Where(!string.IsNullOrEmpty(input.ValueType), u => u.ValueType == input.ValueType)
                                     .Where(u => u.CreatorId == input.CreatorId)
                                     .Where(u => u.CreateTime == input.CreateTime)
                                     .Where(u => u.UpdateId == input.UpdateId)
                                     .Where(u => u.UpdateTime == input.UpdateTime)
                                     .Where(u => u.Sort == input.Sort)
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
            await _ssuIssueExtendAttributeRep.InsertAsync(ssuIssueExtendAttribute, true);

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                //IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.New,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = $"新增详细问题字段【{input.AttributeCode}】"
            }, true);
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
            await _ssuIssueExtendAttributeRep.DeleteAsync(ssuIssueExtendAttribute);

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                //IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.Edit,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = $"删除详细问题字段【{ssuIssueExtendAttribute.AttributeCode}】"
            }, true);
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
            await _ssuIssueExtendAttributeRep.UpdateAsync(ssuIssueExtendAttribute, ignoreNullValues: true);

            await this._ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
            {
                //IssueId = input.Id,
                OperationTypeId = Core.Enum.EnumIssueOperationType.Edit,
                OperationTime = DateTime.Now,
                OperatorName = Helper.Helper.GetCurrentUser().GetNameByEmpId(),
                Content = $"更新详细问题字段【{ssuIssueExtendAttribute.AttributeCode}】"
            }, true);
        }

        [HttpPost($"/SsuIssueExtendAttribute/update-field-struct")]
        public void UpdateFieldStruct(long updateId, EnumModule module, List<FieldStruct> fieldStructs)
        {
            DateTime now = DateTime.Now;

            var collection = this._ssuIssueExtendAttributeRep.DetachedEntities
                .Where(attribute => attribute.Module == module)
                .Where(attribute => fieldStructs.Any(fieldStruct => fieldStruct.FieldCode == attribute.AttributeCode))
                .ToArray();

            Helper.Helper.Assert(collection != null && collection.Length > 0, "");

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

            this._ssuIssueExtendAttributeRep.Entities.UpdateRange(collection);
            this._ssuIssueExtendAttributeRep.Context.SaveChangesAsync();
        }

        [HttpPost($"/SsuIssueExtendAttribute/add-field-struct")]
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

            await this._ssuIssueExtendAttributeRep.Entities.AddRangeAsync(attributes.ToArray());
            await this._ssuIssueExtendAttributeRep.Context.SaveChangesAsync();
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

    }
}
