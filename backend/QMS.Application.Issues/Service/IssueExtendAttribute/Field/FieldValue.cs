namespace QMS.Application.Issues.Field
{
    public class FieldValue : FieldStruct
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        public long IssueId { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
    }
}
