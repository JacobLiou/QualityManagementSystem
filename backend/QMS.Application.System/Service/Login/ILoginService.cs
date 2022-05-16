namespace QMS.Application.System
{
    public interface ILoginService
    {
        string PhoneLogin(string phone, string captcha);
        string SendSMSCode(string phone, int num = 4);
    }
}