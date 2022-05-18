namespace QMS.Application.System
{
    public interface IEmailService
    {
        Task<bool> SendMessage(string[] mailTo, string mailTitle, string mailContent);

        Task<bool> SendMessage(long[] mailTo, string mailTitle, string mailContent);
    }
}