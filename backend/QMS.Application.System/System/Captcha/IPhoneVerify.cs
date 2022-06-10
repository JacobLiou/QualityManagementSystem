namespace QMS.Application.System
{
    public interface IPhoneVerify
    {
        string GetRandomNums(int length = 4);

        CommonOutput VerifyPhoneNums(string phone, string nums);

        string PostSMS(string posturl, string postData);

        CommonOutput SendSMS(string mobile, string context);

        string phoneLogin(string phone, string captcha);

        string SendSMSCode(string phone, int num = 4);
    }
}