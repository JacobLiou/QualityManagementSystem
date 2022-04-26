using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{

    [Table("ssu_issue")]
    [Comment("问题记录")]
    public class SsuIssue : IEntity<IssuesDbContextLocator>, IEntityTypeBuilder<SsuIssue, IssuesDbContextLocator>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("问题编号")]
        public long Id { get; set; }

        [Comment("问题简述")]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Comment("项目编号")]
        public long ProjectId { get; set; }
        //public SsuProject Project { get; set; }

        [Comment("产品编号")]
        public long ProductId { get; set; }
        //public SsuProduct Product { get; set; }

        [Comment("问题模块")]
        public EnumModule Module { get; set; }

        [Comment("问题性质")]
        public EnumConsequence Consequence { get; set; }


        [Comment("问题分类")]
        public EnumIssueClassification IssueClassification { get; set; }


        [Comment("问题来源")]
        public EnumIssueSource Source { get; set; }

        [Comment("问题状态")]
        public EnumIssueStatus Status { get; set; }


        [Comment("提出人")]
        public long CreatorId { get; set; }


        [Comment("提出日期")]
        public DateTime CreateTime { get; set; }


        [Comment("关闭日期")]
        public DateTime? CloseTime { get; set; }

        [Comment("发现人")]
        public long? Discover { get; set; }

        [Comment("发现日期")]
        public DateTime? DiscoverTime { get; set; }

        [Comment("分发人")]
        public long? Dispatcher { get; set; }

        [Comment("分发日期")]
        public DateTime? DispatchTime { get; set; }

        [Comment("预计完成日期")]
        public DateTime? ForecastSolveTime { get; set; }

        [Comment("被抄送人")]
        public long? CC { get; set; }


        [Comment("解决人")]
        public long? Executor { get; set; }

        [Comment("解决日期")]
        public DateTime? SolveTime { get; set; }


        [Comment("验证人")]
        public long? Verifier { get; set; }


        [Comment("验证地点")]
        [MaxLength(200)]
        public string VerifierPlace { get; set; }

        [Comment("验证日期")]
        public DateTime? ValidateTime { get; set; }

        [Comment("软删除")]
        public bool IsDeleted { get; set; } = false;

        public void Configure(EntityTypeBuilder<SsuIssue> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            //entityBuilder.HasMany(i => i.SsuIssueOperations)
            //    .WithOne(i => i.Issue)
            //    .HasForeignKey(i => i.IssueId);

            entityBuilder.HasOne(i => i.SsuIssueDetail)
                .WithOne(i => i.Issue)
                .HasForeignKey<SsuIssueDetail>(i => i.Id);
        }

        [NotMapped]
        public ICollection<SsuIssueOperation> SsuIssueOperations { get; set; }

        [NotMapped]
        public SsuIssueDetail SsuIssueDetail { get; set; }
    }
}
