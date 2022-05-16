using QMS.Core.Enum;

namespace QMS.Application.Issues.Field
{
    public class FieldStruct
    {
        /// <summary>
        /// 模块
        /// </summary>
        public EnumModule Module { get; set; }
        /// <summary>
        /// 字段编号
        /// 新增时忽略该字段
        /// </summary>
        public long FieldId { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段代码
        /// </summary>
        public string FieldCode { get; set; }
        /// <summary>
        /// 字段数据类型
        /// </summary>
        public string FiledDataType { get; set; }
    }
}
