using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Application.Issues.Field;
using QMS.Core;
using QMS.Core.Entity;
using System.Linq.Dynamic.Core;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 详细问题记录服务
    /// </summary>
    [ApiDescriptionSettings("自己的业务", Name = "SsuIssueDetail", Order = 100)]
    public class SsuIssueDetailService : ISsuIssueDetailService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SsuIssueDetail,IssuesDbContextLocator> _ssuIssueDetailRep;
        private readonly IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> _ssuIssueExtendAttrRep;
        private readonly IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> _ssuIssueExtendValueRep;

        public SsuIssueDetailService(
            IRepository<SsuIssueDetail,IssuesDbContextLocator> ssuIssueDetailRep,
            IRepository<SsuIssueExtendAttribute, IssuesDbContextLocator> ssuIssueExtendAttrRep,
            IRepository<SsuIssueExtendAttributeValue, IssuesDbContextLocator> ssuIssueExtendValueRep
        )
        {
            this._ssuIssueDetailRep = ssuIssueDetailRep;
            this._ssuIssueExtendAttrRep = ssuIssueExtendAttrRep;
            this._ssuIssueExtendValueRep = ssuIssueExtendValueRep;
        }

        /// <summary>
        /// 分页查询详细问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueDetail/page")]
        public async Task<PageResult<SsuIssueDetailOutput>> Page([FromQuery] SsuIssueDetailInput input)
        {
            var ssuIssueDetails = await _ssuIssueDetailRep.DetachedEntities
                                     .Where(!string.IsNullOrEmpty(input.Description), u => u.Description == input.Description)
                                     .Where(!string.IsNullOrEmpty(input.Reason), u => u.Reason == input.Reason)
                                     .Where(!string.IsNullOrEmpty(input.Measures), u => u.Measures == input.Measures)
                                     .Where(u => u.Count == input.Count)
                                     .Where(!string.IsNullOrEmpty(input.Batch), u => u.Batch == input.Batch)
                                     .Where(!string.IsNullOrEmpty(input.Result), u => u.Result == input.Result)
                                     .Where(!string.IsNullOrEmpty(input.SolveVersion), u => u.SolveVersion == input.SolveVersion)
                                     .Where(!string.IsNullOrEmpty(input.Comment), u => u.Comment == input.Comment)
                                     .Where(!string.IsNullOrEmpty(input.HangupReason), u => u.HangupReason == input.HangupReason)
                                     .Where(!string.IsNullOrEmpty(input.ExtendAttribute), u => u.ExtendAttribute == input.ExtendAttribute)
                                     .OrderBy(PageInputOrder.OrderBuilder<SsuIssueDetailInput>(input))
                                     .ProjectToType<SsuIssueDetailOutput>()
                                     .ToADPagedListAsync(input.PageNo, input.PageSize);

            return ssuIssueDetails;
        }

        /// <summary>
        /// 增加详细问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueDetail/add")]
        public async Task Add(AddSsuIssueDetailInput input)
        {
            var ssuIssueDetail = input.Adapt<SsuIssueDetail>();
            await _ssuIssueDetailRep.InsertAsync(ssuIssueDetail, true);
        }

        /// <summary>
        /// 删除详细问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueDetail/delete")]
        public async Task Delete(DeleteSsuIssueDetailInput input)
        {
            var ssuIssueDetail = await _ssuIssueDetailRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            await _ssuIssueDetailRep.DeleteAsync(ssuIssueDetail);
        }

        /// <summary>
        /// 更新详细问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/SsuIssueDetail/edit")]
        public async Task Update(UpdateSsuIssueDetailInput input)
        {
            var isExist = await _ssuIssueDetailRep.AnyAsync(u => u.Id == input.Id, false);
            if (!isExist) throw Oops.Oh(ErrorCode.D3000);

            var ssuIssueDetail = input.Adapt<SsuIssueDetail>();
            await _ssuIssueDetailRep.UpdateAsync(ssuIssueDetail,ignoreNullValues:true);
        }

        /// <summary>
        /// 获取详细问题记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueDetail/detail")]
        public async Task<SsuIssueDetailOutput> Get([FromQuery] QueryeSsuIssueDetailInput input)
        {
            SsuIssueDetailOutput detail = (await this._ssuIssueDetailRep.DetachedEntities.FirstOrDefaultAsync(u => u.Id == input.Id)).Adapt<SsuIssueDetailOutput>();

            return detail;
        }

        /// <summary>
        /// 获取详细问题记录列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/SsuIssueDetail/list")]
        public async Task<List<SsuIssueDetailOutput>> List([FromQuery] SsuIssueDetailInput input)
        {
            return await _ssuIssueDetailRep.DetachedEntities.ProjectToType<SsuIssueDetailOutput>().ToListAsync();
        }


        [HttpPost($"/SsuIssueDetail/add-field-value")]
        public async Task AddFieldValue(long IssueId, List<FieldValue> fieldValues)
        {
            DateTime now = DateTime.Now;

            // 找到对应的字段编号
            var array = this._ssuIssueExtendAttrRep.DetachedEntities.Where<SsuIssueExtendAttribute>(field =>
               fieldValues.Any<FieldValue>(value => value.AttributeCode == field.AttributeCode)
           ).ToArray();

            // 根据字段编号和问题Id插入数据
            await this._ssuIssueExtendValueRep.Entities.AddRangeAsync(array.Select<SsuIssueExtendAttribute, SsuIssueExtendAttributeValue>(attribute =>
                 new SsuIssueExtendAttributeValue()
                 {
                     Id = attribute.Id,
                     IssueNum = IssueId,
                     AttibuteValue = fieldValues.FirstOrDefault(value => value.AttributeCode == attribute.AttributeCode).Value
                 })
             );
        }

        [HttpPost($"/SsuIssueDetail/update-field-value")]
        public async Task UpdateFieldValue(long IssueId, List<FieldValue> fieldValues)
        {
            DateTime now = DateTime.Now;

            // 找到对应的字段编号
            var array = this._ssuIssueExtendAttrRep.DetachedEntities.Where<SsuIssueExtendAttribute>(field =>
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

            var values = this._ssuIssueExtendValueRep.Entities
                .Where<SsuIssueExtendAttributeValue>(
                value =>
                value.IssueNum == IssueId
                && array.Any<SsuIssueExtendAttribute>(attribute => attribute.Id == value.Id)
                );


            foreach (var item in values)
            {
                item.AttibuteValue = dic[item.Id];
            }

            this._ssuIssueExtendValueRep.Entities.UpdateRange(values);
            await this._ssuIssueExtendValueRep.Context.SaveChangesAsync();
        }
    }
}
