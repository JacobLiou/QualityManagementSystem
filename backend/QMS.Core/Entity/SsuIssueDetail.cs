using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{

    [Table("ssu_issue_detail")]
    [Comment("详细问题记录")]
    public class SsuIssueDetail : IEntity<IssuesDbContextLocator>, IEntityTypeBuilder<SsuIssueDetail, IssuesDbContextLocator>
    {
        [Key]
        [Comment("问题编号")]
        public long Id { get; set; }

        [ForeignKey(nameof(Id))]
        [NotMapped]
        public SsuIssue Issue { get; set; }

        [Comment("问题详情")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Comment("原因分析")]
        [MaxLength(1000)]
        public string Reason { get; set; }

        [Comment("解决措施")]
        [MaxLength(1000)]
        public string Measures { get; set; }

        [Comment("验证数量")]
        public int? Count { get; set; }

        [Comment("验证批次")]
        [MaxLength(100)]
        public string Batch { get; set; }

        [Comment("验证情况")]
        [MaxLength(1000)]
        public string Result { get; set; }

        [Comment("解决版本")]
        [MaxLength(1000)]
        public string SolveVersion { get; set; }

        [Comment("备注")]
        [MaxLength(300)]
        public string Comment { get; set; }

        [Comment("挂起情况")]
        [MaxLength(1000)]
        public string HangupReason { get; set; }

        /// <summary>
        /// [
        ///     {"Name":"CustomerID","Alias":"客户ID","Value":"100"},
        ///     {"Name":"PCBVersion","Alias":"PCB版本","Value":"AAA"}
        /// ]
        /// </summary>
        [Comment("扩展属性")]
        [MaxLength(1500)]
        public string ExtendAttribute { get; set; }

        /// <summary>
        /// [
        ///     {"IssueId":long, "FileName":originName, "AttachmentId": long, "AttachmentType": 0},
        ///     {"IssueId":long, "FileName":originName, "AttachmentId": long, "AttachmentType": 0}
        /// ]
        /// </summary>
        [Comment("附件信息")]
        [MaxLength(1000)]
        public string Attachments { get; set; }

        public void Configure(EntityTypeBuilder<SsuIssueDetail> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
        }
    }
}
