﻿using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Security.Claims;

namespace Furion.Extras.Admin.NET
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
    {
        private readonly IEventPublisher _eventPublisher;

        public LogExceptionHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var userContext = App.User;
            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:ExLog",
                new SysLogEx
                {
                    Account = userContext?.FindFirstValue(ClaimConst.CLAINM_ACCOUNT),
                    Name = userContext?.FindFirstValue(ClaimConst.CLAINM_NAME),
                    ClassName = context.Exception.TargetSite.DeclaringType?.FullName,
                    MethodName = context.Exception.TargetSite.Name,
                    ExceptionName = context.Exception.Message,
                    ExceptionMsg = context.Exception.Message,
                    ExceptionSource = context.Exception.Source,
                    StackTrace = context.Exception.StackTrace,
                    ParamsObj = context.Exception.TargetSite.GetParameters().ToString(),
                    ExceptionTime = DateTimeOffset.Now
                }));

            // 写日志文件
            Log.Error(context.Exception.ToString());
        }
    }
}