using System.Text.Json.Serialization;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 长时间未处理催办DTO
    /// </summary>
    public class UrgeRuleDto
    {
        /// <summary>
        /// 问题状态值
        /// </summary>
        [JsonPropertyName("Procedure")]
        public List<int> Procedure { get; set; }

        /// <summary>
        /// 催办提示人员字段
        /// </summary>
        [JsonPropertyName("noticeField")]
        public string NoticeField { get; set; }

        /// <summary>
        /// 0-人员字段本人，1-人员字段上级，以此类推
        /// </summary>
        [JsonPropertyName("userType")]
        public int UserType { get; set; }

        /// <summary>
        /// 时间开始字段
        /// </summary>
        [JsonPropertyName("startTiemField")]
        public string StartTiemField { get; set; }

        /// <summary>
        /// 时间结束字段
        /// </summary>
        [JsonPropertyName("endTiemField")]
        public string EndTiemField { get; set; }

        /// <summary>
        /// 催办规则
        /// </summary>
        [JsonPropertyName("judge")]
        public string Judge { get; set; }

        /// <summary>
        /// 催办时间点
        /// </summary>
        [JsonPropertyName("times")]
        public List<string> Times { get; set; }
    }
}