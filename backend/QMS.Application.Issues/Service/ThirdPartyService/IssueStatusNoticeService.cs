using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QMS.Core;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 企业微信、邮件推送
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "IssueStatusNotice", Order = 100)]
    [Route("issue/[controller]")]
    public class IssueStatusNoticeService : IDynamicApiController, IScoped
    {
 
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEventPublisher _eventPublisher;

        public IssueStatusNoticeService( IHttpContextAccessor contextAccessor, IEventPublisher eventPublisher)
        {

            _contextAccessor = contextAccessor;
            _eventPublisher = eventPublisher;
        }

        public class NoticeMsgInput
        {
            /// <summary>
            /// 要推送的目标页面链接
            /// </summary>
            [Required]
            public string Url { get; set; } = "http://qms.sofarsolar.com:8001/issue/detail/288141121613894";

            /// <summary>
            /// 消息标题
            /// </summary>
            [Required]
            public string Title { get; set; } = "测试企业微信消息";

            /// <summary>
            /// 消息正文
            /// </summary>
            [Required]
            public string Content { get; set; } = "系统无法登录问题";

            /// <summary>
            /// 用户id数组
            /// </summary>
            [Required]
            public List<string> UserIdList { get; set; }
        }

        /// <summary>
        /// 通过事件总线发送通知
        /// </summary>
        /// <param name="url">推送目标页面链接</param>
        /// <param name="userIdList">用户id数组</param>
        /// <returns></returns>
        [HttpPost("sendMessageToApp")]
        public async Task SendNoticeAsync(NoticeMsgInput input)
        {
            Helper.Helper.CheckInput(input);

            Helper.Helper.Assert(input.Url != null && input.UserIdList != null && input.UserIdList.Count > 0, "消息推送输入参数不合法");

            NoticeContext notice = new NoticeContext();
            notice.Title = input.Title;
            notice.Content = input.Content;
            notice.PublicUserId = CurrentUserInfo.UserId;
            notice.PageUrl = input.Url;
            notice.NoticeUserIdList = input.UserIdList;
            notice.Type = (int)NoticeType.NOTICE;

            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:Notice", notice));

            // 写日志文件
            Log.Information(notice.ToString());
        }
    }
}
