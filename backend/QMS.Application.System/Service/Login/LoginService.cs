using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.System
{
    /// <summary>
    /// 登录服务类
    /// </summary>
    [ApiDescriptionSettings(Name = "login")]
    [AllowAnonymous]
    public class LoginService : IDynamicApiController, ITransient, ILoginService
    {
        private readonly IPhoneVerify _phone;
        private readonly string Context = "您好，您的验证码是：{0}【首航新能源】";    //手机验证码格式
        private readonly IEmailApplpy _email;
        public LoginService(IPhoneVerify PhoneVerify, IEmailApplpy email)
        {
            _phone = PhoneVerify;
            _email = email;
        }

        /// <summary>
        ///手机号登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        [HttpPost("system/login/phoneLogin")]
        public string PhoneLogin(string phone, string captcha)
        {
            return _phone.phoneLogin(phone, captcha);
        }


        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="num">验证码个数</param>
        /// <returns></returns>
        [HttpPost]
        public string SendSMSCode(string phone, int num = 4)
        {
            return _phone.SendSMSCode(phone, num);
        }
        [HttpGet("system/login/getEmailCaptcha")]
        public string getEmailCaptcha (string email)
        {
            return _email.SendEmailCode(email, 4);
        }

        
    }
}