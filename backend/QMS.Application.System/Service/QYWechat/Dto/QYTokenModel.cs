using System.Text.Json.Serialization;

namespace QMS.Application.System
{
    /// <summary>
    /// AccessToken参数
    /// </summary>
    public class QYTokenModel
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        /// <summary>
        ///错误信息
        /// </summary>
        [JsonPropertyName("errmsg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// access_token
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [JsonPropertyName("expires_in")]
        public dynamic ExpiresIn { get; set; }
    }

    public static class AccessTokenModelExtensions
    {
        /// <summary>
        /// 获取的Token是否包含错误
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        public static bool HasError(this QYTokenModel accessTokenModel)
        {
            return accessTokenModel == null ||
                   string.IsNullOrEmpty(accessTokenModel.AccessToken) ||
                   !accessTokenModel.ErrMsg.Equals("ok", StringComparison.InvariantCultureIgnoreCase) ||
                   accessTokenModel.ErrorCode != 0;
        }
    }
}