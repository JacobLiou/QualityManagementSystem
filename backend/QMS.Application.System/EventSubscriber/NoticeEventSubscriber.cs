using Furion;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service.Notice;
using Furion.JsonSerialization;
using Microsoft.Extensions.DependencyInjection;
using QMS.Core;
using Serilog;
using System.Net;

namespace QMS.Application.System.EventSubscriber
{
    /// <summary>
    /// 消息订阅服务
    /// </summary>
    public class NoticeEventSubscriber : IEventSubscriber
    {
        public IServiceProvider Services { get; }

        public NoticeEventSubscriber(IServiceProvider services)
        {
            Services = services;
        }

        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("Create:Notice")]
        public async Task CreateNotice(EventHandlerExecutingContext context)
        {
            var notice = (NoticeContext)context.Source.Payload;
            IgnoreErrors(SendWeChatMessage(notice));
            IgnoreErrors(SendEmail(notice));
            IgnoreErrors(SendNotice(notice));
            // 写日志文件
            Log.Information("消息发送:" + JSON.Serialize(notice));
        }

        /// <summary>
        /// 问题发送微信消息
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public async Task SendWeChatMessage(NoticeContext notice)
        {
            using var scope = Services.CreateScope();
            var WeChatOAuth = scope.ServiceProvider.GetRequiredService<IQYWeChatOAuth>();
            await WeChatOAuth.QYWechatSendMessage(notice.NoticeUserIdList, null, null, notice.Title, notice.Content, notice.PageUrl);
        }

        /// <summary>
        /// 问题发送邮件
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public async Task SendEmail(NoticeContext notice)
        {
            using var scope = Services.CreateScope();
            var EmailApply = scope.ServiceProvider.GetRequiredService<IEmailApplpy>();
            //构建邮箱跳转信息界面，格式类似为<a href=http://qms.sofarsolar.com:8002/problemAdd&UserID=1234567896325&state=FromEamil>消息标题</a>,供前端识别跳转
            var pageUrl = "<a href=\"" + notice.PageUrl + "&UserID=" + notice.PublicUserId + "&state=FromEmail" + "\">" + notice.Title + "</a>";
            await EmailApply.SendEmail(notice.NoticeUserIdList, notice.Title, notice.Content + pageUrl);
        }

        /// <summary>
        /// 问题添加至公告通知
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public async Task SendNotice(NoticeContext notice)
        {
            using var scope = Services.CreateScope();
            var noticeService = scope.ServiceProvider.GetRequiredService<ISysNoticeService>();
            //构建显示跳转链接
            var content = "<a href=\"" + notice.PageUrl + "\" target=\"_blank\">" + notice.Content + "</a><br>";
            await noticeService.AddNotice(new Furion.Extras.Admin.NET.Service.AddNoticeInput
            {
                Title = notice.Title,
                Content = content,
                Status = NoticeStatus.PUBLIC,
                Type = 1,
                NoticeUserIdList = notice.NoticeUserIdList.Select(u => Convert.ToInt64(u)).ToList()
            });
        }

        /// <summary>
        /// 忽略错误继续执行
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public bool IgnoreErrors(Task operation)
        {
            if (operation == null)
                return false;
            try
            {
                operation.Start();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}