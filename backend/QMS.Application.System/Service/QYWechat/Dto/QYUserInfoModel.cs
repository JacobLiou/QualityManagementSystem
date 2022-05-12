using Furion.DataEncryption;
using Furion.Extras.Admin.NET;
using System.Text.Json.Serialization;

namespace QMS.Application.System
{
    /// <summary>
    /// 企业微信用户详细信息，属性名称对照返回json
    /// </summary>
    public class QYUserInfoModel
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonPropertyName("errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>

        [JsonPropertyName("errmsg")]
        public string ErrMsg { get; set; }


        /// <summary>
        /// 企业微信UserId
        /// </summary>
        [JsonPropertyName("userid")]
        public string Account { get; set; }


        /// <summary>
        /// 微信名
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        [JsonPropertyName("mobile")]
        public string Phone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [JsonPropertyName("gender")]
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }


        [JsonPropertyName("status")]
        public int Status { get; set; }


        [JsonPropertyName("isleader")]
        public int Isleader { get; set; }

        [JsonPropertyName("telephone")]
        public string Tel { get; set; }


        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }

    public static class QYUserInfoModelExtensions
    {
        /// <summary>
        /// 获取的用户是否包含错误
        /// </summary>
        /// <param name="qyuserIdModel"></param>
        /// <returns></returns>
        public static bool HasError(this QYUserInfoModel qyuserIdModel)
        {
            return qyuserIdModel == null;
        }
    }
}