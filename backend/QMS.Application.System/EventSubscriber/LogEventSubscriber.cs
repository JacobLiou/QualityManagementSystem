using Furion.EventBus;
using Microsoft.Extensions.DependencyInjection;
using QMS.Application.System.Service.Log;
using QMS.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System.EventSubscriber
{
    internal class LogEventSubscriber : IEventSubscriber
    {
        public IServiceProvider Services { get; }

        public LogEventSubscriber(IServiceProvider services)
        {
            Services = services;
        }
        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [EventSubscribe("Create:LogDebug")]
        public async Task CreateLogDebug(EventHandlerExecutingContext context)
        {
            var log = (SysLogDebug)context.Source.Payload;
            using var scope = Services.CreateScope();
            var logDebugService = scope.ServiceProvider.GetRequiredService<ILogDebugService>();
            // 写日志
            await logDebugService.AddLogDebug(log);
        }


    }
}
