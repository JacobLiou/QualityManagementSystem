using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace QMS.Application.System
{
    /// <summary>
    /// 用户注册服务
    /// </summary>
    [ApiDescriptionSettings(Name = "register")]
    public class RegisterService : IDynamicApiController, ITransient, IRegisterService
    {
        private readonly IRepository<SysUser> _sysUserRep; // 用户表仓储
        private readonly IPhoneVerify _phone;
        private readonly string Context = "您好，您的验证码是：{0}【首航新能源】";    //手机验证码格式

        public RegisterService(IRepository<SysUser> sysUser, IPhoneVerify PhoneVerify)
        {
            _sysUserRep = sysUser;
            _phone = PhoneVerify;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="input">界面输入参数(账户，邮箱，密码，手机号)</param>
        /// <returns></returns>
        [HttpPost("system/register/userRegister")]
        public bool RegisterUser(RegisterInput input)
        {
            //判断验证码是否正确
            CommonOutput output = new CommonOutput();
            output = _phone.VerifyPhoneNums(input.Captcha);
            if (output.Success == false)
            {
                throw Oops.Oh(output.Message);
            }

            //判断用户是否已经存在
            var user = _sysUserRep.Where(u => u.Account.Equals(input.Account)).FirstOrDefault();
            if (user != null)
            {
                throw Oops.Oh("该账号已存在");
            }
            user = _sysUserRep.Where(u => u.Phone.Equals(input.Phone)).FirstOrDefault();
            if (user != null)
            {
                throw Oops.Oh("该手机号已存在对应用户");
            }
            user = _sysUserRep.Where(u => u.Email.Equals(input.Email)).FirstOrDefault();
            if (user != null)
            {
                throw Oops.Oh("该邮箱已存在对应用户");
            }

            user = input.Adapt<SysUser>();
            user.Password = MD5Encryption.Encrypt(user.Password);
            var newUser = _sysUserRep.InsertNowAsync(user).Result;
            if (newUser == null)
            {
                throw Oops.Oh("注册失败,请重新尝试");
            }
            else
            {
                return true;
            }
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
    }
}