using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    /// <summary>
    /// 用户注册详细信息
    /// </summary>
    public class RegisterInput
    {
        /// <summary>
        /// 账户
        /// </summary>
        [JsonPropertyName("account")]
        public string Account { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>

        [JsonPropertyName("email")]
        public string Email { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        [JsonPropertyName("mobile")]
        public string Phone { get; set; }



        /// <summary>
        /// 验证码
        /// </summary>
        [JsonPropertyName("captcha")]
        public string Captcha { get; set; }
    }
}