using QMS.Core.Enum;

namespace QMS.Application.Issues.Field
{
    public class FieldStruct
    {
        public EnumModule Module { get; set; }
        /// <summary>
        /// 新增时忽略该字段
        /// </summary>
        public long FieldId { get; set; }
        public string FieldName { get; set; }
        public string FieldCode { get; set; }
        public string FiledDataType { get; set; }
    }
}
