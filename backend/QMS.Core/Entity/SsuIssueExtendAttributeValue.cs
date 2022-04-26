using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{
    [Table("ssu_issue_extend_attribute_value")]
    [Comment("问题扩展属性值")]
    public class SsuIssueExtendAttributeValue : IEntity<IssuesDbContextLocator>, IEntityTypeBuilder<SsuIssueExtendAttributeValue, IssuesDbContextLocator>
    {
        [Comment("字段编号")]
        public long Id { get; set; }

        [ForeignKey(nameof(Id))]
        [NotMapped]
        public SsuIssueExtendAttribute IssueExtendAttribute { get; set; }

        [Comment("问题编号")]
        public long IssueNum { get; set; }

        [ForeignKey(nameof(IssueNum))]
        [NotMapped]
        public SsuIssue Issue { get; set; }

        [Comment("字段值")]
        [MaxLength(200)]
        [Required]
        public string AttibuteValue { get; set; }

        public void Configure(EntityTypeBuilder<SsuIssueExtendAttributeValue> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasKey(entity => new { entity.Id, entity.IssueNum });
        }
    }
}
