using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Application.Issues;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.IssueService.Dto.Detail;
using QMS.Application.Issues.IssueService.Dto.Field;
using QMS.Application.Issues.IssueService.Dto.New;
using QMS.Application.Issues.IssueService.Dto.QueryList;
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
    public class MyIssue : IMyIssuesService, IDynamicApiController, ITransient
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

        [HttpPost($"/{nameof(MyIssue)}/add-common")]
        public async Task AddCommon(InCommonNewWithOutId input)
        {
            var ssuIssues = input.Adapt<SsuIssue>();
            await this._commonIssuesRep.InsertAsync(ssuIssues);
        }

        [HttpPost($"/{nameof(MyIssue)}/add-detail")]
        public async Task AddDetail(OutputDetailIssue input)
        {
            var ssuIssues = input.Adapt<SsuIssueDetail>();
            await this._detailIssuesRep.InsertAsync(ssuIssues);
        }

        [HttpPost($"/{nameof(MyIssue)}/delete")]
        public async Task Delete(BaseId input)
        {
            var ssuIssues = await this._commonIssuesRep.FirstOrDefaultAsync(u => u.Id == input.Id);

            ssuIssues.IsDeleted = true;

            await this._commonIssuesRep.UpdateAsync(ssuIssues);
        }

        [HttpPost($"/{nameof(MyIssue)}/update-common")]
        public async Task Update(InCommonNew input)
        {
            var isExist = await this._commonIssuesRep.DetachedEntities.AnyAsync(u => u.Id == input.Id);

            Helper.Assert(isExist, $"问题编号{input.Id}不存在");

            var ssuIssues = input.Adapt<SsuIssue>();
            await this._commonIssuesRep.UpdateAsync(ssuIssues, ignoreNullValues: true);
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
                     IssueId = IssueId,
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
                value.IssueId == IssueId
                && array.Any<SsuIssueExtendAttribute>(attribute => attribute.Id == value.Id)
                );


            foreach (var item in values)
            {
                item.AttibuteValue = dic[item.Id];
            }

            this._fieldValueIssuesRep.Entities.UpdateRange(values);
            await this._fieldValueIssuesRep.Context.SaveChangesAsync();
        }

        [HttpGet($"/{nameof(MyIssue)}/get")]
        public async Task<OutputDetailIssue> Get([FromQuery] BaseId input)
        {
            return (await this._detailIssuesRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<OutputDetailIssue>();
        }

        [HttpGet($"/{nameof(MyIssue)}/page")]
        public async Task<PageResult<OutputGeneralIssue>> PageWithGeneralCondition([FromQuery] BaseQueryModel input)
        {
            var ssuIssuess = await this._commonIssuesRep.DetachedEntities
                                     .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                     .Where(input.Module >= 0, u => (int)u.Module == input.Module)
                                     .Where(input.Consequence >= 0, u => (int)u.Consequence == input.Consequence)
                                     .Where(input.Status >= 0, u => (int)u.Status == input.Status)
                                     .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                     //.OrderBy(PageInputOrder.OrderBuilder<SsuIssuesInput>(input))
                                     .ProjectToType<OutputGeneralIssue>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet($"/{nameof(MyIssue)}/page-creator")]
        public async Task<PageResult<OutputGeneralIssue>> PageByCreator([FromQuery] QueryListByCreator input)
        {
            var ssuIssuess = await this._commonIssuesRep.DetachedEntities
                                     .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                     .Where(input.Module >= 0, u => (int)u.Module == input.Module)
                                     .Where(input.Consequence >= 0, u => (int)u.Consequence == input.Consequence)
                                     .Where(input.Status >= 0, u => (int)u.Status == input.Status)
                                     .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                     .Where(input.CreatorId>0, u=>u.CreatorId==input.CreatorId)
                                     //.OrderBy(PageInputOrder.OrderBuilder<SsuIssuesInput>(input))
                                     .ProjectToType<OutputGeneralIssue>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet($"/{nameof(MyIssue)}/page-dispatcher")]
        public async Task<PageResult<OutputGeneralIssue>> PageByDispatcher([FromQuery] QueryListByDispatcher input)
        {
            var ssuIssuess = await this._commonIssuesRep.DetachedEntities
                                    .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                    .Where(input.Module >= 0, u => (int)u.Module == input.Module)
                                    .Where(input.Consequence >= 0, u => (int)u.Consequence == input.Consequence)
                                    .Where(input.Status >= 0, u => (int)u.Status == input.Status)
                                    .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                    .Where(input.Dispatcher > 0, u => u.Dispatcher == input.Dispatcher)
                                    //.OrderBy(PageInputOrder.OrderBuilder<SsuIssuesInput>(input))
                                    .ProjectToType<OutputGeneralIssue>()
                                    .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet($"/{nameof(MyIssue)}/page-executor")]
        public async Task<PageResult<OutputGeneralIssue>> PageByExector([FromQuery] QueryListByExecutor input)
        {
            var ssuIssuess = await this._commonIssuesRep.DetachedEntities
                                    .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                    .Where(input.Module >= 0, u => (int)u.Module == input.Module)
                                    .Where(input.Consequence >= 0, u => (int)u.Consequence == input.Consequence)
                                    .Where(input.Status >= 0, u => (int)u.Status == input.Status)
                                    .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                    .Where(input.Executor > 0, u => u.Dispatcher == input.Executor)
                                    .ProjectToType<OutputGeneralIssue>()
                                    .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

            [HttpGet($"/{nameof(MyIssue)}/page-solved")]
        public async Task<PageResult<OutputGeneralIssue>> PageBySolved([FromQuery] QueryListInSolved input)
        {
            var ssuIssuess = await this._commonIssuesRep.DetachedEntities
                                   .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                   .Where(input.Module >= 0, u => (int)u.Module == input.Module)
                                   .Where(input.Consequence >= 0, u => (int)u.Consequence == input.Consequence)
                                   .Where(input.Status >= 0, u => (int)u.Status == input.Status)
                                   .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                   .ProjectToType<OutputGeneralIssue>()
                                   .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet($"/{nameof(MyIssue)}/page-unsolve")]
        public async Task<PageResult<OutputGeneralIssue>> PageByUnSolvd([FromQuery] QueryListInUnSolve input)
        {
            var ssuIssuess = await this._commonIssuesRep.DetachedEntities
                                   .Where(input.ProjectId > 0, u => u.ProductId == input.ProjectId)
                                   .Where(input.Module >= 0, u => (int)u.Module == input.Module)
                                   .Where(input.Consequence >= 0, u => (int)u.Consequence == input.Consequence)
                                   .Where(input.Status >= 0, u => (int)u.Status == input.Status)
                                   .Where(!string.IsNullOrEmpty(input.KeyWord), u => u.Title.Contains(input.KeyWord))
                                   .ProjectToType<OutputGeneralIssue>()
                                   .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssuess;
        }

        [HttpGet($"/{nameof(MyIssue)}/list")]
        public async Task<List<OutputGeneralIssue>> List([FromQuery] BaseQueryModel input)
        {
            return await this._commonIssuesRep.DetachedEntities.ProjectToType<OutputGeneralIssue>().ToListAsync();
        }
    }
}
