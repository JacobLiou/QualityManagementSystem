using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题扩展属性值服务
    /// </summary>
    [ApiDescriptionSettings("自己的业务", Name = "SsuIssueExtendAttributeValue", Order = 100)]
    public class SsuIssueExtendAttributeValueService : ISsuIssueExtendAttributeValueService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssueExtendAttributeValue,IssuesDbContextLocator> _ssuIssueExtendAttributeValueRep;

        public SsuIssueExtendAttributeValueService(
            IRepository<SsuIssueExtendAttributeValue,IssuesDbContextLocator> ssuIssueExtendAttributeValueRep
        )
        {
            _ssuIssueExtendAttributeValueRep = ssuIssueExtendAttributeValueRep;
        }

        /// <summary>
        /// 分页查询问题扩展属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueExtendAttributeValue/page")]
        public async Task<PageResult<SsuIssueExtendAttributeValueOutput>> Page([FromQuery] SsuIssueExtendAttributeValueInput input)
        {
            var ssuIssueExtendAttributeValues = await _ssuIssueExtendAttributeValueRep.DetachedEntities
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuIssueExtendAttributeValueInput>(input))
                                     .ProjectToType<SsuIssueExtendAttributeValueOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssueExtendAttributeValues;
        }

        /// <summary>
        /// 增加问题扩展属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueExtendAttributeValue/add")]
        public async Task Add(AddSsuIssueExtendAttributeValueInput input)
        {
            var ssuIssueExtendAttributeValue = input.Adapt<SsuIssueExtendAttributeValue>();
            await _ssuIssueExtendAttributeValueRep.InsertAsync(ssuIssueExtendAttributeValue);
        }

        /// <summary>
        /// 删除问题扩展属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueExtendAttributeValue/delete")]
        public async Task Delete(DeleteSsuIssueExtendAttributeValueInput input)
        {
            var ssuIssueExtendAttributeValue = await _ssuIssueExtendAttributeValueRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            var ssuIssueExtendAttributeValue = await _ssuIssueExtendAttributeValueRep.FirstOrDefaultAsync(u => u.IssueId == input.IssueId);
            await _ssuIssueExtendAttributeValueRep.DeleteAsync(ssuIssueExtendAttributeValue);
        }

        /// <summary>
        /// 更新问题扩展属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueExtendAttributeValue/edit")]
        public async Task Update(UpdateSsuIssueExtendAttributeValueInput input)
        {
            var isExist = await _ssuIssueExtendAttributeValueRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuIssueExtendAttributeValue = input.Adapt<SsuIssueExtendAttributeValue>();
            await _ssuIssueExtendAttributeValueRep.UpdateAsync(ssuIssueExtendAttributeValue,ignoreNullValues:true);
        }

        /// <summary>
        /// 获取问题扩展属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueExtendAttributeValue/detail")]
        public async Task<SsuIssueExtendAttributeValueOutput> Get([FromQuery] QueryeSsuIssueExtendAttributeValueInput input)
        {
            return (await _ssuIssueExtendAttributeValueRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuIssueExtendAttributeValueOutput>();
            return (await _ssuIssueExtendAttributeValueRep.DetachedEntities.FirstOrDefaultAsync(u => u.IssueId == input.IssueId)).Adapt<SsuIssueExtendAttributeValueOutput>();
        }

        /// <summary>
        /// 获取问题扩展属性值列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueExtendAttributeValue/list")]
        public async Task<List<SsuIssueExtendAttributeValueOutput>> List([FromQuery] SsuIssueExtendAttributeValueInput input)
        {
            return await _ssuIssueExtendAttributeValueRep.DetachedEntities.ProjectToType<SsuIssueExtendAttributeValueOutput>().ToListAsync();
        }    

    }
}
