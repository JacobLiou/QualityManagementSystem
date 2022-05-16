namespace QMS.Application.System
{
    public interface IEmailService
    {
        bool SendMessage(string[] mailTo, string mailTitle, string mailContent);
    }
}