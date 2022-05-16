using System.Text.Json.Serialization;


namespace QMS.Application.System
{
    /// <summary>
    /// 企业微信发送消息返回结果
    /// </summary>
    public class QYWechatResult
    {
        /// <summary>
        ///返回码
        /// </summary>
        [JsonPropertyName("errcode")]
        public int Errcode { get; set; }

        /// <summary>
        ///对返回码的文本描述内容
        /// </summary>
        [JsonPropertyName("errmsg")]
        public string Errmsg { get; set; }


        /// <summary>
        ///不合法的userid，不区分大小写，统一转为小写
        /// </summary>
        [JsonPropertyName("invaliduser")]
        public string Invaliduser { get; set; }


        /// <summary>
        ///不合法的partyid
        /// </summary>
        [JsonPropertyName("invalidparty")]
        public string Invalidparty { get; set; }

        /// <summary>
        ///不合法的标签id
        /// </summary>
        [JsonPropertyName("invalidtag")]
        public string Invalidtag { get; set; }


        /// <summary>
        ///消息id，用于撤回应用消息
        /// </summary>
        [JsonPropertyName("msgid")]
        public string Msgid { get; set; }

        /// <summary>
        ///仅消息类型为“按钮交互型”，“投票选择型”和“多项选择型”的模板卡片消息返回，应用可使用response_code调用更新模版卡片消息接口，24小时内有效，且只能使用一次
        /// </summary>
        [JsonPropertyName("response_code")]
        public string Response_code { get; set; }
    }
}