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
        public int Id { get; set; }

        /// <summary>
        /// 问题简述
        /// </summary>
        [Comment("问题简述")]
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        [Comment("问题描述")]
        public string Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Comment("状态")]
        public int Status { get; set; }



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
