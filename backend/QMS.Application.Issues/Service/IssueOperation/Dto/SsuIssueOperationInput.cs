using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题操作记录输入参数
    /// </summary>
    public class IssueOperationInput : PageInputBase
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public virtual long IssueId { get; set; }
        
        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual Core.Enum.EnumIssueOperationType OperationTypeId { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; set; }
        
        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime OperationTime { get; set; }
        
    }

    public class AddIssueOperationInput : IssueOperationInput
    {
    }

    public class DeleteIssueOperationInput : BaseId
    {
    }

    public class UpdateIssueOperationInput : IssueOperationInput
    {
        /// <summary>
        /// 问题操作记录编号
        /// </summary>
        [Required(ErrorMessage = "问题操作记录编号不能为空")]
        public long Id { get; set; }
        
    }

    public class QueryeIssueOperationInput : BaseId
    {

    }
}
