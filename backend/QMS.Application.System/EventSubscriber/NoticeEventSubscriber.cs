using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Microsoft.Extensions.DependencyInjection;
using QMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System.EventSubscriber
{
    public class NoticeEventSubscriber : IEventSubscriber
    {
        public IServiceProvider Services { get; }

        public NoticeEventSubscriber(IServiceProvider services)
        {
            Services = services;
        }
        

        [EventSubscribe("Create:Notice")]
        public async Task CreateNotice(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var WeChatOAuth = scope.ServiceProvider.GetRequiredService<IQYWeChatOAuth>();

            var notice = (NoticeContext)context.Source.Payload;


            await WeChatOAuth.QYWechatSendMessage(new[]{ "SZSF000559" }, null, null, notice.Title, notice.Content, notice.PageUrl);

        }

    }
}
