using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core;
using QMS.Core.Entity;
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

        public SsuIssueExtendAttributeService(
            IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> ssuIssueExtendAttributeRep
        )
        {
            _ssuIssueExtendAttributeRep = ssuIssueExtendAttributeRep;
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
