using Furion;
using Furion.EventBus;
using Furion.JsonSerialization;
using Microsoft.Extensions.DependencyInjection;
using QMS.Core;
using Serilog;

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
            SendWeChatMessage(notice);
            SendEmail(notice);
            // 写日志文件
            Log.Information("消息发送:" + JSON.Serialize(notice));
        }

        /// <summary>
        /// 发送微信消息
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
        /// 发送邮件
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public async Task SendEmail(NoticeContext notice)
        {
            using var scope = Services.CreateScope();
            var EmailApply = scope.ServiceProvider.GetRequiredService<IEmailApplpy>();
            notice.PageUrl = "<a href=\"" + notice.PageUrl + "\">" + notice.Title + "</a>";   //构建邮箱跳转信息界面
            await EmailApply.SendEmail(notice.NoticeUserIdList, notice.Title, notice.Content + notice.PageUrl);
        }
    }
}