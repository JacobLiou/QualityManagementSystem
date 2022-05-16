namespace QMS.Application.System
{
    public interface IEmailApplpy
    {
        bool SendEmail(string[] mailTo, string mailTitle, string mailContent);
    }
}