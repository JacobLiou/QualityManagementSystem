using Furion.DependencyInjection;
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
        private readonly string StmpServer = "smtp.sofarsolar.com";                          //smtp服务器地址
        private readonly string MailAccount = "platformservice@sofarsolar.com";              //邮箱账号
        private readonly string Pwd = "yxs123456#";                                           //邮箱密码
        private readonly string NickName = "质量平台通知服务";                              //邮件发送名称

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">接收人邮件列表</param>
        /// <param name="mailTitle">发送邮件标题</param>
        /// <param name="mailContent">发送邮件内容</param>
        /// <returns></returns>
        public bool SendEmail(string[] mailTo, string mailTitle, string mailContent)
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
                for (int i = 0; i < mailTo.Length; i++)
                {
                    //MailMessage mailMessage = new MailMessage(MailAccount, mailTo[i]);      //实例化邮件信息实体并设置发送方和接收方
                    MailAddress to = new MailAddress(mailTo[i]);
                    MailMessage mailMessage = new MailMessage(from, to);
                    mailMessage.Subject = mailTitle;                                        //标题
                    mailMessage.Body = mailContent;                                         //内容
                    mailMessage.BodyEncoding = Encoding.UTF8;                               //编码
                    mailMessage.IsBodyHtml = true;                                          //是否为HTML格式
                    mailMessage.Priority = MailPriority.Normal;                             //优先级
                    //smtpClient.Send(mailMessage);
                    smtpClient.SendMailAsync(mailMessage);                                  //发送邮件
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
                Serilog.Log.Error("SendMails Error：" + e.Error.ToString());
                throw Oops.Oh("SendMails Error：" + e.Error.ToString());
            }
        }
    }
}