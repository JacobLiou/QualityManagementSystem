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
        private readonly ISsuEmpService _ssuEmpService;
        private readonly ISsuProjectService _ssuProjectService;
        private readonly ICacheService _cacheService;
        private readonly int CacheMinute = 30;

        public SsuProductService(
            IRepository<SsuProduct, MasterDbContextLocator> ssuProductRep, IRepository<SsuProductUser> ssuProductUserRep, IRepository<SysUser> ssuSysuser,
            ISysEmpService sysEmpService, ICacheService cacheService, ISsuEmpService ssuEmpService, ISsuProjectService ssuProjectService
        )
        {
            _ssuProductRep = ssuProductRep;
            _ssuProductUserRep = ssuProductUserRep;
            _ssuSysuser = ssuSysuser;
            _sysEmpService = sysEmpService;
            _cacheService = cacheService;
            _ssuEmpService = ssuEmpService;
            _ssuProjectService = ssuProjectService;
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
            //获取产品的关联人员列表
            foreach (SsuProductOutput output in ssuProducts.Rows)
            {
                //设置产品负责人名称
                output.DirectorName = output.DirectorId.GetUserNameById();
                //设置产品所属项目名称
                output.ProjectName = output.ProjectId.GetProjectNameById();
                var userList = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId == output.Id).Select(u => u.EmployeeId).ToList();
                if (userList != null && userList.Count > 0)
                {
                    output.UserList = userList.GetUserListById().Adapt<List<UserOutput>>();
                }
            }
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
            var result = await _ssuProductRep.InsertAsync(ssuProduct);

            if (result != null && result.Entity.Id != 0 && input.UserIdList.Count() > 0)
            {
                var list = input.UserIdList.Select(u => new SsuProductUser() { ProductId = result.Entity.Id, EmployeeId = u });
                await _ssuProductUserRep.InsertAsync(list);
            }
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

            var ssuProductUser = await _ssuProductUserRep.DetachedEntities.FirstOrDefaultAsync(u => u.ProductId == input.Id);
            if (ssuProductUser != null)
            {
                await _ssuProductUserRep.DeleteAsync(ssuProductUser);
            }
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
            if (!isExist) throw Oops.Oh("产品组不存在");

            var ssuProduct = input.Adapt<SsuProduct>();
            await _ssuProductRep.UpdateAsync(ssuProduct, ignoreNullValues: true);
            await _cacheService.SetCacheByMinutes(CoreCommonConst.PRODUCTID + input.Id, ssuProduct, CacheMinute);

            //更新产品组人员列表
            var existsList = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId.Equals(input.Id)).Select(u => u.EmployeeId).ToList();
            //获取在新增列表中存在，但是不存在于人员组中的ID，执行新增操作
            var intersectionList = input.UserIdList.Except(existsList).Select(u => new SsuProductUser() { ProductId = input.Id, EmployeeId = u }).ToList();
            if (intersectionList != null && intersectionList.Count() > 0)
            {
                await _ssuProductUserRep.InsertAsync(intersectionList);
            }

            //获取在产品组中存在的ID，但是不存在于新增的ID列表中的ID，执行删除操作
            var differenceList = existsList.Except(input.UserIdList).ToList();
            var ssuProductUser = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId == input.Id && differenceList.Contains(u.EmployeeId)).ToList();
            if (ssuProductUser != null && ssuProductUser.Count() > 0)
            {
                await _ssuProductUserRep.DeleteAsync(ssuProductUser);
            }
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuProduct/detail")]
        public async Task<SsuProductOutput> Get([FromQuery] QueryeSsuProductInput input)
        {
            var detail = await _ssuProductRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (detail == null)
            {
                throw Oops.Oh("人员组不存在");
            }
            var result = detail.Adapt<SsuProductOutput>();
            var productUserId = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId == input.Id)
                .Select(u => u.EmployeeId).ToList();
            result.UserList = _ssuEmpService.GetUserList(productUserId).Result.Values.ToList().Adapt<List<UserOutput>>();
            //设置产品负责人名称
            result.DirectorName = result.DirectorId.GetUserNameById();
            //设置产品所属项目名称
            result.ProjectName = result.ProjectId.GetProjectNameById();
            return result;
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
            var result = await _ssuProductRep.DetachedEntities.ProjectToType<SsuProductOutput>().ToListAsync();
            foreach (SsuProductOutput output in result)
            {
                //设置产品负责人名称
                output.DirectorName = output.DirectorId.GetUserNameById();
                //设置产品所属项目名称
                output.ProjectName = output.ProjectId.GetProjectNameById();
                var userList = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId == output.Id).Select(u => u.EmployeeId).ToList();
                if (userList != null && userList.Count > 0)
                {
                    output.UserList = userList.GetUserListById().Adapt<List<UserOutput>>();
                }
            }
            return result;
        }

        /// <summary>
        ///根据产品ID获取产品对应的人员列表
        /// </summary>
        /// <param name="productInput">产品分页参数</param>
        /// <returns></returns>
        [HttpGet("/SsuProduct/getproductusers")]
        public async Task<PageResult<UserOutput>> GetProductUsers([FromQuery] SsuProductUserInput productInput)
        {
            PageResult<UserOutput> result = new PageResult<UserOutput>();
            List<UserOutput> list = new List<UserOutput>();
            var userIds = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId.Equals(productInput.productId)).Select(u => u.EmployeeId);
            if (userIds != null && userIds.Any())
            {
                result = _ssuSysuser.DetachedEntities.Where(u => userIds.Contains(u.Id) && u.AdminType != AdminType.SuperAdmin && u.IsDeleted == false)
                    .Select(u => u.Adapt<UserOutput>())
                    .ToADPagedList(productInput.PageNo, productInput.PageSize);

                result.Rows.ToList().ForEach(
                    delegate (UserOutput user)
                    {
                        user.SysEmpInfo = _sysEmpService.GetEmpInfo(Convert.ToInt64(user.Id)).Result;
                    }
                    );
            }
            return result;
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
            var existsList = _ssuProductUserRep.DetachedEntities.Where(u => u.ProductId.Equals(productId)).Select(u => u.EmployeeId);
            //获取在新增列表中存在，但是不存在于产品组中的ID
            var intersectionList = userIds.Except(existsList);
            foreach (long employeeId in intersectionList)
            {
                SsuProductUser projectUser = new SsuProductUser();
                projectUser.ProductId = productId;
                projectUser.EmployeeId = employeeId;
                list.Add(projectUser);
            }
            if (list != null && list.Count > 0)
            {
                await _ssuProductUserRep.InsertAsync(list);
            }

            //获取在产品组中存在的ID，但是不存在于新增的ID列表中的ID
            var differenceList = existsList.Except(userIds);
            if (differenceList != null && differenceList.Count() > 0)
            {
                foreach (long employeeId in differenceList)
                {
                    await _ssuProductUserRep.DeleteAsync(employeeId);
                }
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