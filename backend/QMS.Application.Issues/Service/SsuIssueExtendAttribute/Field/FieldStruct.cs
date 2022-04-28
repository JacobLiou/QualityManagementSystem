using QMS.Core.Enum;

namespace QMS.Application.Issues.Field
{
    public class FieldStruct
    {
        public EnumModule Module { get; set; }
        public string FieldName { get; set; }
        public string FieldCode { get; set; }
        public string FiledDataType { get; set; }
    }
}
