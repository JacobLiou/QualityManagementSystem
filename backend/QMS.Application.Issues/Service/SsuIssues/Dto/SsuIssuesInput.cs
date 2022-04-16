using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题管理输入参数
    /// </summary>
    public class SsuIssuesInput : PageInputBase
    {
        /// <summary>
        /// 问题简述
        /// </summary>
        public virtual string Title { get; set; }
        
        /// <summary>
        /// 问题描述
        /// </summary>
        public virtual string Description { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }
        
    }

    public class AddSsuIssuesInput : SsuIssuesInput
    {
    }

    public class DeleteSsuIssuesInput : BaseId
    {
    }

    public class UpdateSsuIssuesInput : SsuIssuesInput
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        public int Id { get; set; }
        
    }

    public class QueryeSsuIssuesInput : BaseId
    {

    }
}
