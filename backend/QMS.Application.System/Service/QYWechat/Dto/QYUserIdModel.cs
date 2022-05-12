using System.Text.Json.Serialization;

namespace QMS.Application.System
{
    /// <summary>
    /// 企业微信用户ID
    /// </summary>
    public class QYUserIdModel
    {
        /// <summary>
        /// 企业微信用户ID
        /// </summary>
        [JsonPropertyName("UserId")]
        public string UserId { get; set; }

        [JsonPropertyName("DeviceId")]
        public string DeviceId { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        [JsonPropertyName("errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        [JsonPropertyName("errmsg")]
        public string ErrMsg { get; set; }
    }

    public static class QYUserIdModelExtensions
    {
        /// <summary>
        /// 获取的用户是否包含错误
        /// </summary>
        /// <param name="qyuserIdModel"></param>
        /// <returns></returns>
        public static bool HasError(this QYUserIdModel qyuserIdModel)
        {
            return qyuserIdModel == null ||
                   !qyuserIdModel.ErrMsg.Equals("ok", StringComparison.InvariantCultureIgnoreCase) ||
                   qyuserIdModel.ErrCode != 0;
        }
    }
}