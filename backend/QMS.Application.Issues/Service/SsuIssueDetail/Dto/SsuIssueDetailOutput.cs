using System;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 详细问题记录输出参数
    /// </summary>
    public class SsuIssueDetailOutput
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 问题详情
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 原因分析
        /// </summary>
        public string Reason { get; set; }
        
        /// <summary>
        /// 解决措施
        /// </summary>
        public string Measures { get; set; }
        
        /// <summary>
        /// 验证数量
        /// </summary>
        public int Count { get; set; }
        
        /// <summary>
        /// 验证批次
        /// </summary>
        public string Batch { get; set; }
        
        /// <summary>
        /// 验证情况
        /// </summary>
        public string Result { get; set; }
        
        /// <summary>
        /// 解决版本
        /// </summary>
        public string SolveVersion { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        
        /// <summary>
        /// 挂起情况
        /// </summary>
        public string HangupReason { get; set; }
        
        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ExtendAttribute { get; set; }
        
    }
}
