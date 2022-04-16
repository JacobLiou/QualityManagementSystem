using System;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题管理输出参数
    /// </summary>
    public class SsuIssuesOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 问题简述
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 问题描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        
    }
}
