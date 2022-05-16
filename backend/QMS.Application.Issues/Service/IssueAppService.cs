using Furion;
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
    [ApiDescriptionSettings("问题管理Demo", Name = "Issue", Order = 100)]

    public class IssueAppService : IDynamicApiController
    {
        private readonly IHttpProxy _http;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEventPublisher _eventPublisher;

        public IssueAppService(IHttpProxy http, IHttpContextAccessor contextAccessor, IEventPublisher eventPublisher)
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
        public async Task SendNoticeAsync()
        {
            NoticeContext notice = new NoticeContext();
            notice.Title = "测试企业微信消息";
            notice.Content = "系统无法登录问题";
            notice.PublicUserId = CurrentUserInfo.UserId;
            notice.PageUrl = "http://qms.sofarsolar.com:8001/issue/detail/288141121613894";
            notice.NoticeUserIdList = null;
            notice.Type = (int)NoticeType.NOTICE;

            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:Notice", notice));

            // 写日志文件
            Log.Information(notice.ToString());
        }

        [HttpGet("/Issue/TestUserGroup")]
        public async Task<List<GroupUserOutput>> GetUserGroup()
        {
            return await this.GroupUserOutputs();
        }

        public async Task<List<GroupUserOutput>> GroupUserOutputs()
        {
            var request = _contextAccessor.HttpContext.Request;
            var authHeader = request.Headers["Authorization"];

            var response = await "http://localhost:5566/System/group/UserGroup".SetHeaders(new
            {
                Authorization = authHeader
            }).GetAsStringAsync();

            var context = JsonConvert.DeserializeObject<ApiModel<List<GroupUserOutput>>>(response);
            return context.data;

        }

     

    }


    public class ApiModel<T>
    {
        public bool success { get; set; }
        public int code { get; set; }
        public T data { get; set; }
        public string message { get; set; }
    }
}
