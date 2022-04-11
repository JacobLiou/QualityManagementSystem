namespace Furion.Extras.Admin.NET
{
    public interface IGeneralCaptcha
    {
        ClickWordCaptchaResult CheckCode(GeneralCaptchaInput input);

        ClickWordCaptchaResult CreateCaptchaImage(int length = 4);
    }
}