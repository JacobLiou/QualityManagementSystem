using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furion.Extras.Admin.NET.Service.Notice
{
    /// <summary>
    /// 通知公告服务
    /// </summary>
    [ApiDescriptionSettings(Name = "Notice", Order = 100)]
    public class SysNoticeService : ISysNoticeService, IDynamicApiController, ITransient
    {
        private readonly IRepository<SysNotice> _sysNoticeRep;  // 通知公告表仓储
        private readonly IRepository<SysNoticeUser> _sysNoticeUserRep;  // 通知公告用户表仓储
        private readonly IRepository<SysEmp> _sysEmpRep;
        private readonly ISysNoticeUserService _sysNoticeUserService;

        public SysNoticeService(IRepository<SysNotice> sysNoticeRep,
                                IRepository<SysNoticeUser> sysNoticeUserRep,
                                IRepository<SysEmp> sysEmpRep,
                                ISysNoticeUserService sysNoticeUserService)
        {
            _sysNoticeRep = sysNoticeRep;
            _sysNoticeUserRep = sysNoticeUserRep;
            _sysEmpRep = sysEmpRep;
            _sysNoticeUserService = sysNoticeUserService;
        }

        /// <summary>
        /// 分页查询通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysNotice/page")]
        public async Task<PageResult<SysNotice>> QueryNoticePageList([FromQuery] NoticePageInput input)
        {
            var searchValue = !string.IsNullOrEmpty(input.SearchValue?.Trim());
            var notices = await _sysNoticeRep.DetachedEntities
                                             .Where(searchValue, u => EF.Functions.Like(u.Title, $"%{input.SearchValue.Trim()}%") ||
                                                                      EF.Functions.Like(u.Content, $"%{input.SearchValue.Trim()}%"))
                                             .Where(input.Type > 0, u => u.Type == input.Type)
                                             .Where(u => u.Status != NoticeStatus.DELETED)
                                             //通知公告管理应只有发布人可以管理自己发布的，其他人只能在已收公告中看到
                                             .Where(u => u.PublicUserId == CurrentUserInfo.UserId)
                                             .ToADPagedListAsync(input.PageNo, input.PageSize);
            return notices;
        }

        /// <summary>
        /// 增加通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysNotice/add")]
        public async Task AddNotice(AddNoticeInput input)
        {
            if (input.Status != NoticeStatus.DRAFT && input.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.D7000);

            var notice = input.Adapt<SysNotice>();
            await UpdatePublicInfo(notice);
            // 如果是发布，则设置发布时间
            if (input.Status == NoticeStatus.PUBLIC)
                notice.PublicTime = DateTimeOffset.Now;
            var newItem = await notice.InsertNowAsync();
            //如果是草稿则将状态标记为暂不通知，下次更改状态后再修改为未更改状态
            if (notice.Status != NoticeStatus.DRAFT)
            {
                // 通知到的人
                var noticeUserIdList = input.NoticeUserIdList;
                var noticeUserStatus = NoticeUserStatus.UNREAD;
                await _sysNoticeUserService.Add(newItem.Entity.Id, noticeUserIdList, noticeUserStatus);
            }
            else
            {
                var noticeUserIdList = input.NoticeUserIdList;
                var noticeUserStatus = NoticeUserStatus.NONOTICE;
                await _sysNoticeUserService.Add(newItem.Entity.Id, noticeUserIdList, noticeUserStatus);
            }
        }

        /// <summary>
        /// 删除通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysNotice/delete")]
        public async Task DeleteNotice(DeleteNoticeInput input)
        {
            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (notice.Status != NoticeStatus.DRAFT && notice.Status != NoticeStatus.CANCEL) // 只能删除草稿和撤回的公告
                throw Oops.Oh(ErrorCode.D7001);
            if (notice.PublicUserId != CurrentUserInfo.UserId)
                throw Oops.Oh(ErrorCode.D7003);
            await notice.DeleteAsync();
        }

        /// <summary>
        /// 更新通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysNotice/edit")]
        public async Task UpdateNotice(UpdateNoticeInput input)
        {
            if (input.Status != NoticeStatus.DRAFT && input.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.D7000);

            //  非草稿状态
            if (input.Status != NoticeStatus.DRAFT)
                throw Oops.Oh(ErrorCode.D7002);
            // 如果发布者非本人则不能修改
            SysNotice noticeInDb = await _sysNoticeRep.DetachedEntities.Where(u => u.Id == input.Id).FirstOrDefaultAsync();
            if (noticeInDb.PublicUserId != CurrentUserInfo.UserId)
                throw Oops.Oh(ErrorCode.D7003);

            var notice = input.Adapt<SysNotice>();
            if (input.Status == NoticeStatus.PUBLIC)
            {
                notice.PublicTime = DateTimeOffset.Now;
                await UpdatePublicInfo(notice);
            }
            await notice.UpdateAsync();
            //如果修改后的状态为非草稿状态则通知
            if (notice.Status != NoticeStatus.DRAFT)
            {
                // 通知到的人
                var noticeUserIdList = input.NoticeUserIdList;
                var noticeUserStatus = NoticeUserStatus.UNREAD;
                await _sysNoticeUserService.Update(input.Id, noticeUserIdList, noticeUserStatus);
            }
        }

        /// <summary>
        /// 获取通知公告详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysNotice/detail")]
        public async Task<NoticeDetailOutput> GetNotice([FromQuery] QueryNoticeInput input)
        {
            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == input.Id);

            // 获取通知到的用户
            var noticeUserList = await _sysNoticeUserService.GetNoticeUserListByNoticeId(input.Id);
            var noticeUserIdList = new List<string>();
            var noticeUserReadInfoList = new List<NoticeUserRead>();
            if (noticeUserList != null)
            {
                noticeUserList.ForEach(u =>
                {
                    noticeUserIdList.Add(u.UserId.ToString());
                    var noticeUserRead = new NoticeUserRead
                    {
                        UserId = u.UserId,
                        UserName = CurrentUserInfo.Name,
                        ReadStatus = u.ReadStatus,
                        ReadTime = u.ReadTime
                    };
                    noticeUserReadInfoList.Add(noticeUserRead);
                });
            }
            var noticeResult = notice.Adapt<NoticeDetailOutput>();
            noticeResult.NoticeUserIdList = noticeUserIdList;
            noticeResult.NoticeUserReadInfoList = noticeUserReadInfoList;
            if (noticeResult.Status == NoticeStatus.CANCEL)
            {
                noticeResult.Content = "<h1 style=\"text-align: center; \">该内容已被发布者撤回</h1>";
            }
            // 如果该条通知公告为已发布，则将当前用户的该条通知公告设置为已读
            if (notice.Status == NoticeStatus.PUBLIC || notice.Status == NoticeStatus.CANCEL)
                await _sysNoticeUserService.Read(notice.Id, CurrentUserInfo.UserId, NoticeUserStatus.READ);
            return noticeResult;
        }

        /// <summary>
        /// 修改通知公告状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/sysNotice/changeStatus")]
        public async Task ChangeStatus(ChangeStatusNoticeInput input)
        {
            // 状态应为撤回或删除或发布
            if (input.Status != NoticeStatus.CANCEL && input.Status != NoticeStatus.DELETED && input.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.D7000);

            var noticeuser = await _sysNoticeUserRep.DetachedEntities.Where(u => u.NoticeId == input.Id).Select(u => u.UserId).ToListAsync();

            var notice = await _sysNoticeRep.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (notice.PublicUserId != CurrentUserInfo.UserId)
            {
                throw Oops.Oh(ErrorCode.D7003);
            }
            notice.Status = input.Status;
            if (input.Status == NoticeStatus.CANCEL)
            {
                notice.CancelTime = DateTimeOffset.Now;
            }
            else if (input.Status == NoticeStatus.PUBLIC)
            {
                notice.PublicTime = DateTimeOffset.Now;
            }
            await notice.UpdateAsync();
            if (notice.Status != NoticeStatus.DRAFT)
            {
                // 通知到的人
                var noticeUserIdList = noticeuser;
                var noticeUserStatus = NoticeUserStatus.UNREAD;
                await _sysNoticeUserService.Update(input.Id, noticeUserIdList, noticeUserStatus);
            }
        }

        /// <summary>
        /// 获取接收的通知公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysNotice/received")]
        public async Task<PageResult<NoticeReceiveOutput>> ReceivedNoticePageList([FromQuery] NoticePageInput input)
        {
            var searchValue = !string.IsNullOrEmpty(input.SearchValue?.Trim());
            var notices = await _sysNoticeRep.DetachedEntities.Join(_sysNoticeUserRep.DetachedEntities, u => u.Id, e => e.NoticeId, (u, e) => new { u, e })
                                             .Where(u => u.e.UserId == CurrentUserInfo.UserId)
                                             .Where(searchValue, u => EF.Functions.Like(u.u.Title, $"%{input.SearchValue.Trim()}%") ||
                                                                      EF.Functions.Like(u.u.Content, $"%{input.SearchValue.Trim()}%"))
                                             .Where(input.Type > 0, u => u.u.Type == input.Type)
                                             .Where(u => u.u.Status != NoticeStatus.DELETED && u.u.Status != NoticeStatus.DRAFT)
                                             .Select(u => u.u.Adapt(u.e.Adapt<NoticeReceiveOutput>()))
                                             .ToADPagedListAsync(input.PageNo, input.PageSize);
            return notices;
        }

        /// <summary>
        /// 更新发布信息
        /// </summary>
        /// <param name="notice"></param>
        [NonAction]
        private async Task UpdatePublicInfo(SysNotice notice)
        {
            var currUserId = CurrentUserInfo.UserId;
            var emp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == currUserId, false);
            notice.PublicUserId = currUserId;
            notice.PublicUserName = CurrentUserInfo.Name;
            notice.PublicOrgId = emp.OrgId;
            notice.PublicOrgName = emp.OrgName;
        }

        /// <summary>
        /// 未处理消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysNotice/unread")]
        public async Task<dynamic> UnReadNoticeList([FromQuery] NoticeInput input)
        {
            var dic = typeof(NoticeType).EnumToList();
            var notices = await (from n in _sysNoticeRep.AsQueryable()
                                 join u in _sysNoticeUserRep.AsQueryable() on n.Id equals u.NoticeId
                                 where u.UserId == CurrentUserInfo.UserId
                                 && u.ReadStatus == NoticeUserStatus.UNREAD
                                 orderby n.CreatedTime descending
                                 select new NoticeReceiveOutput
                                 {
                                     CancelTime = n.CancelTime,
                                     Id = n.Id,
                                     Content = n.Content,
                                     Title = n.Title,
                                     Status = n.Status,
                                     Type = n.Type,
                                     PublicOrgId = n.PublicOrgId,
                                     PublicOrgName = n.PublicOrgName,
                                     PublicTime = n.PublicTime,
                                     PublicUserId = n.PublicUserId,
                                     PublicUserName = n.PublicUserName,
                                     ReadStatus = u.ReadStatus,
                                     ReadTime = u.ReadTime
                                 })
                                 .Skip(input.PageNo > 0 ? input.PageNo - 1 : input.PageNo)
                                 .Take(input.PageSize)
                                 .ToListAsync();

            var count = notices.Count();

            List<dynamic> noticeClays = new List<dynamic>();
            int index = 0;
            foreach (var item in dic)
            {
                noticeClays.Add(
                    new
                    {
                        Index = index++,
                        Key = item.Describe,
                        Value = item.Value,
                        NoticeData = notices.Where(m => m.Type == item.Value).ToList()
                    }
                );
            }
            return new
            {
                Rows = noticeClays,
                TotalRows = count
            };
        }
    }
}