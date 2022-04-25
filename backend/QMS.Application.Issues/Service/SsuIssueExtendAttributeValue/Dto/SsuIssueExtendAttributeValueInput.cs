using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题扩展属性值输入参数
    /// </summary>
    public class SsuIssueExtendAttributeValueInput : PageInputBase
    {
        /// <summary>
        /// 字段值
        /// </summary>
        public virtual string AttibuteValue { get; set; }
        
    }

    public class AddSsuIssueExtendAttributeValueInput : SsuIssueExtendAttributeValueInput
    {
    }

    public class DeleteSsuIssueExtendAttributeValueInput : BaseId
    {
    }

    public class UpdateSsuIssueExtendAttributeValueInput : SsuIssueExtendAttributeValueInput
    {
        /// <summary>
        /// 字段编号
        /// </summary>
        [Required(ErrorMessage = "字段编号不能为空")]
        public long Id { get; set; }
        
        /// <summary>
        /// 问题编号
        /// </summary>
        [Required(ErrorMessage = "问题编号不能为空")]
        public long IssueId { get; set; }
        
    }

    public class QueryeSsuIssueExtendAttributeValueInput : BaseId
    {

    }
}
