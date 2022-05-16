namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题扩展属性输出参数
    /// </summary>
    public class IssueExtendAttributeOutput
    {
        /// <summary>
        /// 字段编号
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 模块编号
        /// </summary>
        public Core.Enum.EnumModule Module { get; set; }
        
        /// <summary>
        /// 字段名
        /// </summary>
        public string AttibuteName { get; set; }
        
        /// <summary>
        /// 字段代码
        /// </summary>
        public string AttributeCode { get; set; }
        
        /// <summary>
        /// 字段值类型
        /// </summary>
        public string ValueType { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        public long CreatorId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 更新人
        /// </summary>
        public long UpdateId { get; set; }
        
        /// <summary>
        /// 提出日期
        /// </summary>
        public DateTime UpdateTime { get; set; }
        
        /// <summary>
        /// 排序优先级
        /// </summary>
        public int Sort { get; set; }
        
    }
}
