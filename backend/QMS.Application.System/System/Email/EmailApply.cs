﻿using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace QMS.Application.System
{
    /// <summary>
    /// 邮件功能
    /// </summary>
    public class EmailApplpy : ITransient, IEmailApplpy
    {
        private static IEventPublisher _eventPublisher;
        private readonly IRepository<SysUser> _sysUserRep;  // 用户表仓储
        private readonly string StmpServer = "smtp.sofarsolar.com";                          //smtp服务器地址
        private readonly string MailAccount = "platformservice@sofarsolar.com";              //邮箱账号
        private readonly string Pwd = "yxs123456#";                                           //邮箱密码
        private readonly string NickName = "质量平台通知服务";                              //邮件发送名称
        private readonly string Context = "您好，您的验证码是：{0}【首航新能源】";    //手机验证码格式
        private readonly ICacheService _cache;
        private readonly int CacheMinute = 1;

        public EmailApplpy(IRepository<SysUser> sysUserRep, ICacheService cache, IEventPublisher eventPublisher)
        {
            _sysUserRep = sysUserRep;
            _cache = cache;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="userId">用户ID列表</param>
        /// <param name="mailTitle">邮件标题</param>
        /// <param name="mailContent">邮件内容</param>
        /// <returns></returns>
        public async Task<bool> SendEmail(IEnumerable<string> userId, string mailTitle, string mailContent)
        {
            var mailTo = _sysUserRep.DetachedEntities.Where(u => userId.Contains(u.Id.ToString())).Select(u => u.Email).ToList();
            if (mailTo == null || mailTo.Count() == 0)
            {
                throw Oops.Oh($"该用户不存在对应的邮箱");
            }
            return await SendEmail(mailTo, mailTitle, mailContent);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">接收人邮件列表</param>
        /// <param name="mailTitle">发送邮件标题</param>
        /// <param name="mailContent">发送邮件内容</param>
        /// <returns></returns>
        private async Task<bool> SendEmail(List<string> mailTo, string mailTitle, string mailContent)
        {
            try
            {
                //邮件服务设置
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;             //发送方式
                smtpClient.Host = StmpServer;                                       //SMTP服务器
                smtpClient.EnableSsl = false;                                       //不启用安全加密连接SSL
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(MailAccount, Pwd);
                smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                MailAddress from = new MailAddress(MailAccount, NickName);
                foreach (string address in mailTo)
                {
                    //MailMessage mailMessage = new MailMessage(MailAccount, mailTo[i]);      //实例化邮件信息实体并设置发送方和接收方
                    MailAddress to = new MailAddress(address);
                    MailMessage mailMessage = new MailMessage(from, to);
                    mailMessage.Subject = mailTitle;                                        //标题
                    mailMessage.Body = mailContent;                                         //内容
                    mailMessage.BodyEncoding = Encoding.UTF8;                               //编码
                    mailMessage.IsBodyHtml = true;                                          //是否为HTML格式
                    mailMessage.Priority = MailPriority.Normal;                             //优先级
                    //smtpClient.Send(mailMessage);
                    await smtpClient.SendMailAsync(mailMessage);                                  //发送邮件
                }
                return true;
            }
            catch (SmtpException ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }

        /// <summary>
        /// 邮件异步发送回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // 增加错误日志
                _eventPublisher.PublishAsync(new ChannelEventSource("Create:ExLog",
                new SysLogEx
                {
                    Account = CurrentUserInfo.Account,
                    Name = CurrentUserInfo.Name,
                    ClassName = "EmailApplpy",
                    MethodName = "SendEmail",
                    ExceptionName = "邮件发送失败",
                    ExceptionMsg = e.Error.ToString(),
                    ExceptionSource = "SendEmail",
                    StackTrace = e.GetDescription(),
                    ParamsObj = "",
                    ExceptionTime = DateTimeOffset.Now
                }));
                throw Oops.Oh("SendMails Error：" + e.Error.ToString());
            }
        }

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="num">验证码个数</param>
        /// <returns></returns>
        public string SendEmailCode(string email, int num = 4)
        {
            if (num <= 0)
            {
                num = 4;
            }
            IList<string> list = new List<string>();
            list.Add(email);
            string code = GetRandomNums(num);
            string context = String.Format(Context, code);
            _cache.SetCacheByMinutes(CacheKeys.CACHE_PHONE_CODE + "_" + email, code, CacheMinute);   //设置缓存时间为一分钟
            CommonOutput output = new CommonOutput();
            output.Success = SendEmail(list, context, context).Result;
            output.Message = "发送成功！";
            return output.Message;
        }

        /// <summary>
        /// 生成一定长度的随机数字串，默认值为4位
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetRandomNums(int length = 4)
        {
            var chars = new StringBuilder();
            char[] character = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
            Random rnd = new();
            // 生成验证码字符串
            for (int i = 0; i < length; i++)
            {
                chars.Append(character[rnd.Next(character.Length)]);
            }

            return chars.ToString();
        }
    }
}