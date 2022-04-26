using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 详细问题记录输入参数
    /// </summary>
    public class SsuIssueDetailInput : PageInputBase
    {
        /// <summary>
        /// 问题详情
        /// </summary>
        public virtual string Description { get; set; }
        
        /// <summary>
        /// 原因分析
        /// </summary>
        public virtual string Reason { get; set; }
        
        /// <summary>
        /// 解决措施
        /// </summary>
        public virtual string Measures { get; set; }
        
        /// <summary>
        /// 验证数量
        /// </summary>
        public virtual int Count { get; set; }
        
        /// <summary>
        /// 验证批次
        /// </summary>
        public virtual string Batch { get; set; }
        
        /// <summary>
        /// 验证情况
        /// </summary>
        public virtual string Result { get; set; }
        
        /// <summary>
        /// 解决版本
        /// </summary>
        public virtual string SolveVersion { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Comment { get; set; }
        
        /// <summary>
        /// 挂起情况
        /// </summary>
        public virtual string HangupReason { get; set; }
        
        /// <summary>
        /// 扩展属性
        /// </summary>
        public virtual string ExtendAttribute { get; set; }
        
    }

    public class AddSsuIssueDetailInput : SsuIssueDetailInput
    {
    }

    public class DeleteSsuIssueDetailInput : BaseId
    {
    }

    public class UpdateSsuIssueDetailInput : SsuIssueDetailInput
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        [Required(ErrorMessage = "问题编号不能为空")]
        public long Id { get; set; }
        
    }

    public class QueryeSsuIssueDetailInput : BaseId
    {

    }
}
