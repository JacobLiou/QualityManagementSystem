using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface IAuthService
    {
        Task<ClickWordCaptchaResult> GetCaptcha();

        Task<bool> GetCaptchaOpen();

        Task<LoginOutput> GetLoginUserAsync();

        string LoginAsync([FromBody] LoginInput input);

        Task LogoutAsync();

        Task<ClickWordCaptchaResult> VerificationCode(ClickWordCaptchaInput input);
    }
}