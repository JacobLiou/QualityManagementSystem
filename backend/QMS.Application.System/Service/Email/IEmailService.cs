namespace QMS.Application.System
{
    public interface IEmailService
    {
        Task<bool> SendMessage(IEnumerable<string> mailTo, string mailTitle, string mailContent);

        //Task<bool> SendMessage(IEnumerable<long> mailTo, string mailTitle, string mailContent);
    }
}