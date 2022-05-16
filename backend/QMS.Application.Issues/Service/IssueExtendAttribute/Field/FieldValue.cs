namespace QMS.Application.Issues.Field
{
    public class FieldValue
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        public long IssueId { get; set; }
        /// <summary>
        /// 创建属性时生成的属性ID
        /// </summary>
        public long AttributeId { get; set; }
        /// <summary>
        /// 属性代码
        /// </summary>
        public string AttributeCode { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 属性值类型，对应创建时下拉列表选择的值
        /// </summary>
        public string ValueType { get; set; }
    }
}
