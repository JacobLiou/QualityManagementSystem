using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{
    [Table("ssu_issue_operation")]
    [Comment("问题操作记录")]
    public class SsuIssueOperation : IEntity<IssuesDbContextLocator>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("问题操作记录编号")]
        public long Id { get; set; }

        [Comment("问题编号")]
        public long IssueId { get; set; }

        [ForeignKey(nameof(IssueId))]
        [NotMapped]
        public SsuIssue Issue { get; set; }

        [Comment("操作类型")]
        [MaxLength(200)]
        public EnumIssueOperationType OperationTypeId { get; set; }

        [Comment("操作人")]
        [MaxLength(50)]
        public string OperatorName { get; set; }

        [Comment("内容")]
        public string Content { get; set; }

        [Comment("时间")]
        [Required]
        public DateTime OperationTime { get; set; }


        [Comment("软删除")]
        public bool IsDeleted { get; set; } = false;
    }
}
