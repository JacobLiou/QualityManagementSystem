using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Entity.Common;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    /// <summary>
    /// 产品服务
    /// </summary>
    [ApiDescriptionSettings(Name = "SsuProduct", Order = 100)]
    public class SsuProductService : ISsuProductService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuProduct,MasterDbContextLocator> _ssuProductRep;

        public SsuProductService(
            IRepository<SsuProduct,MasterDbContextLocator> ssuProductRep
        )
        {
            _ssuProductRep = ssuProductRep;
        }

        /// <summary>
        /// 分页查询产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProduct/page")]
        public async Task<PageResult<SsuProductOutput>> Page([FromQuery] SsuProductInput input)
        {
            var ssuProducts = await _ssuProductRep.DetachedEntities
                                     .Where(!string.IsNullOrEmpty(input.ProductName), u => u.ProductName == input.ProductName)
                                     .Where(!string.IsNullOrEmpty(input.ProductLine), u => u.ProductLine == input.ProductLine)
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuProductInput>(input))
                                     .ProjectToType<SsuProductOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuProducts;
        }

        /// <summary>
        /// 增加产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuProduct/add")]
        public async Task Add(AddSsuProductInput input)
        {
            var ssuProduct = input.Adapt<SsuProduct>();
            await _ssuProductRep.InsertAsync(ssuProduct);
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuProduct/delete")]
        public async Task Delete(DeleteSsuProductInput input)
        {
            var ssuProduct = await _ssuProductRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await _ssuProductRep.DeleteAsync(ssuProduct);
        }

        /// <summary>
        /// 更新产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuProduct/edit")]
        public async Task Update(UpdateSsuProductInput input)
        {
            var isExist = await _ssuProductRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuProduct = input.Adapt<SsuProduct>();
            await _ssuProductRep.UpdateAsync(ssuProduct,ignoreNullValues:true);
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProduct/detail")]
        public async Task<SsuProductOutput> Get([FromQuery] QueryeSsuProductInput input)
        {
            return (await _ssuProductRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuProductOutput>();
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProduct/list")]
        public async Task<List<SsuProductOutput>> List([FromQuery] SsuProductInput input)
        {
            return await _ssuProductRep.DetachedEntities.ProjectToType<SsuProductOutput>().ToListAsync();
        }    

    }
}
