using Furion.DynamicApiController;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Furion.RemoteRequest.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QMS.Core;
using Serilog;

namespace QMS.Application.Issues
{ /// <summary>
  /// 系统服务接口
  /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "Issue", Order = 100)]
    [Route("issue/[controller]")]
    public class IssueStatusNoticeService : IDynamicApiController
    {
        private readonly IHttpProxy _http;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEventPublisher _eventPublisher;

        public IssueStatusNoticeService(IHttpProxy http, IHttpContextAccessor contextAccessor, IEventPublisher eventPublisher)
        {
            _http = http;
            _contextAccessor = contextAccessor;
            _eventPublisher = eventPublisher;
        }
        /// <summary>
        /// 通过事件总线发送通知
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [HttpGet("sendNotice")]
        public async Task SendNoticeAsync(string url = "http://qms.sofarsolar.com:8001/issue/detail/288141121613894")
        {
            NoticeContext notice = new NoticeContext();
            notice.Title = "测试企业微信消息";
            notice.Content = "系统无法登录问题";
            notice.PublicUserId = CurrentUserInfo.UserId;
            notice.PageUrl = url;
            notice.NoticeUserIdList = null;
            notice.Type = (int)NoticeType.NOTICE;

            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:Notice", notice));

            // 写日志文件
            Log.Information(notice.ToString());
        }
    }
}
