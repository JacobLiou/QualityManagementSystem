using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QMS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Entity
{

    /// <summary>
    /// 员工表
    /// </summary>
    [Table("ssu_issues")]
    [Comment("问题记录")]
    public class SsuIssues : IEntity<IssuesDbContextLocator>, IEntityTypeBuilder<SsuIssues, IssuesDbContextLocator>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("问题编号")]
        public int Id { get; set; }

        [Comment("问题简述")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Comment("问题描述")]
        [MaxLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        [Comment("项目编号")]
        public long ProjectId { get; set; }

        [Comment("产品编号")]
        public long ProductId { get; set; }


        [Comment("问题模块")]
        public long Stage { get; set; }


        [Comment("问题性质")]
        public long Consequence { get; set; }


        [Comment("问题分类")]
        public long IssueClassification { get; set; }


        [Comment("问题来源")]
        public long Source { get; set; }

        [Comment("问题状态")]
        public long Status { get; set; }


        [Comment("提出人")]
        public long CreatorId { get; set; }


        [Comment("提出日期")]
        public DateTime CreateTime { get; set; }


        [Comment("关闭日期")]
        public DateTime CloseTime { get; set; }

        [Comment("发现人")]
        public long Discover { get; set; }

        [Comment("发现日期")]
        public DateTime DisconverTime { get; set; }

        [Comment("分发人")]
        public long Dispatcher { get; set; }

        [Comment("分发日期")]
        public DateTime DispatchTime { get; set; }

        [Comment("预计完成日期")]
        public DateTime ForecastSolveTime { get; set; }


        [Comment("解决人")]
        public long Executor { get; set; }

        [Comment("解决日期")]
        public DateTime SolveTime { get; set; }


        [Comment("验证人")]
        public long Verifier { get; set; }


        [Comment("验证地点")]
        [MaxLength(200)]
        public string VerifierPlace { get; set; }

        [Comment("验证日期")]
        public DateTime ValidateTime { get; set; }

        [Comment("软删除")]
        public bool IsDeleted { get; set; }


        /// <summary>
        /// 多对多配置关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SsuIssues> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {

        }

       
    }
}
