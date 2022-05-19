using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.System
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public class EmailService : IDynamicApiController, ITransient, IEmailService
    {
        private readonly IEmailApplpy _email;

        public EmailService(IEmailApplpy emailApplpy)
        {
            _email = emailApplpy;
        }


        /// <summary>
        ///发送邮件接口
        /// </summary>
        /// <param name="mailTo">接收人邮件列表</param>
        /// <param name="mailTitle">发送邮件标题</param>
        /// <param name="mailContent">发送邮件内容</param>
        /// <returns></returns>
        [HttpPost("system/email/SendMessageAddres")]
        public async Task<bool> SendMessage(IEnumerable<string> mailTo, string mailTitle, string mailContent)
        {
            return await _email.SendEmail(mailTo, mailTitle, mailContent);
        }


        /// <summary>
        ///发送邮件接口
        /// </summary>
        /// <param name="mailTo">用户userID</param>
        /// <param name="mailTitle">发送邮件标题</param>
        /// <param name="mailContent">发送邮件内容</param>
        /// <returns></returns>
        [HttpPost("system/email/SendMessageUserId")]
        public async Task<bool> SendMessage(IEnumerable<long> mailTo, string mailTitle, string mailContent)
        {
            return await _email.SendEmail(mailTo, mailTitle, mailContent);
        }
    }
}