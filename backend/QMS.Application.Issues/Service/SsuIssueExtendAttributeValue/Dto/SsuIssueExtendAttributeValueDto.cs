using System;
using Furion.Extras.Admin.NET;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题扩展属性值输出参数
    /// </summary>
    public class SsuIssueExtendAttributeValueDto
    {
        /// <summary>
        /// 字段编号
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 问题编号
        /// </summary>
        public long IssueId { get; set; }
        
        /// <summary>
        /// 字段值
        /// </summary>
        public string AttibuteValue { get; set; }
        
    }
}
