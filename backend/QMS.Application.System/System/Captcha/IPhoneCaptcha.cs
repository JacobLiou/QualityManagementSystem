namespace QMS.Application.System
{
    public interface IPhoneCaptcha
    {
        string GetRandomNums(int length = 4);

        CommonOutput VerifyPhoneNums(string nums);

        string PostSMS(string posturl, string postData);

        CommonOutput SendSMS(string mobile, string context);
    }
}