using Furion.DynamicApiController;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Furion.RemoteRequest.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QMS.Core;
using Serilog;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "test", Order = 100)]
    [Route("issue/[controller]")]
    public class IssueAppService : IDynamicApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEventPublisher _eventPublisher;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 问题管理应用服务
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="eventPublisher"></param>
        /// <param name="configuration"></param>
        public IssueAppService(IHttpContextAccessor contextAccessor
            , IEventPublisher eventPublisher
            , IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            _eventPublisher = eventPublisher;
            _configuration = configuration;
        }
        /// <summary>
        /// 通过事件总线发送通知
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [HttpGet("sendNotice")]
        public async Task SendNoticeAsync()
        {
            var serviceUrl = _configuration["RemoteServiceHost"].ToString();
            NoticeContext notice = new NoticeContext();
            notice.Title = "测试企业微信消息";
            notice.Content = "系统无法登录问题";
            notice.PublicUserId = CurrentUserInfo.UserId;
            notice.PageUrl = serviceUrl + "issue/detail/288141121613894";
            notice.NoticeUserIdList = null;
            notice.Type = (int)NoticeType.NOTICE;

            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:Notice", notice));

            // 写日志文件
            Log.Information(notice.ToString());
        }

        [HttpGet("testUserGroup")]
        public async Task<List<GroupUserOutput>> GetUserGroup()
        {
            return await this.GroupUserOutputs();
        }

        private async Task<List<GroupUserOutput>> GroupUserOutputs()
        {
            var request = _contextAccessor.HttpContext.Request;
            var authHeader = request.Headers["Authorization"];
            var serviceUrl = _configuration["RemoteServiceHost"].ToString();

            var response = await (serviceUrl + "System/group/UserGroup").SetHeaders(new
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
