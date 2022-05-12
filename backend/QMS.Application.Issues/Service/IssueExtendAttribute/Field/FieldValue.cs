namespace QMS.Application.Issues.Field
{
    public class FieldValue
    {
        public long IssueId { get; set; }
        public long AttributeId { get; set; }
        public string AttributeCode { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
}
