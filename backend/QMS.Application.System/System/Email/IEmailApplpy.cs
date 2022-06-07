namespace QMS.Application.System
{
    public interface IEmailApplpy
    {
        Task<bool> SendEmail(IEnumerable<string> mailTo, string mailTitle, string mailContent);


        Task<bool> SendEmail(IEnumerable<long> userId, string mailTitle, string mailContent);

        string SendEmailCode(string email, int num);
    }
}