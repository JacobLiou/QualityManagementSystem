using System.ComponentModel.DataAnnotations;

namespace Furion.Extras.Admin.NET.Service
{
    /// <summary>
    /// 登录输入参数
    /// </summary>
    public class EmailLoginInput
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        /// <example>superAdmin</example>
        [Required(ErrorMessage = "邮箱不能为空")]
        public string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <example>123456</example>
        [Required(ErrorMessage = "验证码不能为空")]
        public string Captcha { get; set; }
    }
}