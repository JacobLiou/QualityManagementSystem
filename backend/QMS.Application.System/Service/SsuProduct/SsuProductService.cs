using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Entity.Common;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core;
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
        private readonly IRepository<SsuProduct, MasterDbContextLocator> _ssuProductRep;
        private readonly IRepository<SsuProductUser> _ssuProductUserRep;
        private readonly IRepository<SysUser> _ssuSysuser;
        private readonly ISysEmpService _sysEmpService;
        private readonly ICacheService _cacheService;
        private readonly int CacheMinute = 30;

        public SsuProductService(
            IRepository<SsuProduct, MasterDbContextLocator> ssuProductRep, IRepository<SsuProductUser> ssuProductUserRep, IRepository<SysUser> ssuSysuser,
            ISysEmpService sysEmpService, ICacheService cacheService
        )
        {
            _ssuProductRep = ssuProductRep;
            _ssuProductUserRep = ssuProductUserRep;
            _ssuSysuser = ssuSysuser;
            _sysEmpService = sysEmpService;
            _cacheService = cacheService;
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
                                     //.Where(!string.IsNullOrEmpty(input.ProductName), u => u.ProductName == input.ProductName)
                                     .Where(!string.IsNullOrEmpty(input.ProductName), u => u.ProductName.Contains(input.ProductName))
                                     .Where(input.ProductLine != null, u => u.ProductLine == input.ProductLine)
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
            await _ssuProductRep.UpdateAsync(ssuProduct, ignoreNullValues: true);
            await _cacheService.SetCacheByMinutes(CoreCommonConst.PRODUCTID + input.Id, ssuProduct, CacheMinute);
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
        /// 问题管理服务调用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuProduct/list")]
        public async Task<Dictionary<long, SsuProductOutput>> List(IEnumerable<long> input)
        {
            return (await _ssuProductRep.DetachedEntities.Where<SsuProduct>(product => input.Contains(product.Id)).ProjectToType<SsuProductOutput>().ToDictionaryAsync(product => product.Id));
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("/SsuProduct/select")]
        public async Task<List<SsuProductOutput>> Select()
        {
            return await _ssuProductRep.DetachedEntities.ProjectToType<SsuProductOutput>().ToListAsync();
        }

        /// <summary>
        ///根据产品ID获取产品对应的人员列表
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns></returns>
        [HttpGet("/SsuProduct/getproductusers")]
        public async Task<List<UserOutput>> GetProductUsers([FromQuery] long productId)
        {
            List<UserOutput> list = new List<UserOutput>();
            var userIds = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId.Equals(productId)).Select(u => u.EmployeeId);
            if (userIds != null)
            {
                var userList = _ssuSysuser.Where(u => userIds.Contains(u.Id)).ToList();
                if (userList != null)
                {
                    foreach (SysUser user in userList)
                    {
                        UserOutput output = user.Adapt<UserOutput>();
                        output.SysEmpInfo = await _sysEmpService.GetEmpInfo(user.Id);
                        list.Add(output);
                    }
                }
            }
            return list;
        }

        /// <summary>
        ///新增人员列表到产品组
        /// </summary>
        /// <param name="productId">产品组ID</param>
        /// <param name="userIds">人员列表ID</param>
        /// <returns></returns>
        [HttpPost("/SsuProduct/insertproductgroup")]
        public async Task InsertProductGroup(long productId, IEnumerable<long> userIds)
        {
            List<SsuProductUser> list = new List<SsuProductUser>();
            var resultList = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId.Equals(productId)).Select(u => u.EmployeeId);
            foreach (long employeeId in userIds)
            {
                if (!resultList.Contains(employeeId))
                {
                    SsuProductUser projectUser = new SsuProductUser();
                    projectUser.ProductId = productId;
                    projectUser.EmployeeId = employeeId;
                    list.Add(projectUser);
                }
            }
            if (list != null && list.Count > 0)
            {
                await _ssuProductUserRep.InsertAsync(list);
            }
        }

        /// <summary>
        /// 根据产品ID列表获取产品详细信息列表
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        [HttpPost("/SsuProduct/getproductlist")]
        public async Task<Dictionary<long, SsuProduct>> GetProductList(IEnumerable<long> productIds)
        {
            Dictionary<long, SsuProduct> Dcit = new Dictionary<long, SsuProduct>();
            var products = _ssuProductRep.DetachedEntities.Where(u => productIds.Contains(u.Id) && u.IsDeleted == false).ToDictionary(u => u.Id);
            //针对每个产品ID都做一次缓存，所以此处采用循环的方式
            foreach (SsuProduct obj in products.Values)
            {
                var cacheProduct = _cacheService.GetCache<SsuProduct>(CoreCommonConst.PRODUCTID + obj.Id);
                if (cacheProduct.Result != null)
                {
                    Dcit.Add(obj.Id, cacheProduct.Result);
                }
                else
                {
                    Dcit.Add(obj.Id, obj);
                    await _cacheService.SetCacheByMinutes(CoreCommonConst.PRODUCTID + obj.Id, obj, CacheMinute);
                }
            }
            return Dcit;
        }
    }
}