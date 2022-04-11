namespace Furion.Extras.Admin.NET
{
    public interface IClickWordCaptcha
    {
        Task<ClickWordCaptchaResult> CheckCode(ClickWordCaptchaInput input);

        Task<ClickWordCaptchaResult> CreateCaptchaImage(string code, int width, int height, int point = 3);

        string RandomCode(int number);
    }
}