namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题操作记录输出参数
    /// </summary>
    public class IssueOperationOutput
    {
        /// <summary>
        /// 问题操作记录编号
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 问题编号
        /// </summary>
        public long IssueId { get; set; }
        
        /// <summary>
        /// 操作类型
        /// </summary>
        public Core.Enum.EnumIssueOperationType OperationTypeId { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }

    }
}
