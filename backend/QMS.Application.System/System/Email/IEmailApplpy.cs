namespace QMS.Application.System
{
    public interface IEmailApplpy
    {
        Task<bool> SendEmail(string[] mailTo, string mailTitle, string mailContent);


        Task<bool> SendEmail(long[] userId, string mailTitle, string mailContent);
    }
}