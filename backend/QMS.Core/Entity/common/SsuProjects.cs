﻿using Furion.DatabaseAccessor;
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

namespace QMS.Core.Entity.common
{

    /// <summary>
    /// 项目表
    /// </summary>
    [Table("ssu_projects")]
    [Comment("项目表")]
    public class SsuProjects : IEntity, IEntityTypeBuilder<SsuProjects, MasterDbContextLocator>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("项目编号")]
        public int Id { get; set; }

        [Comment("项目名称")]
        [MaxLength(200)]
        public string ProjectName { get; set; }

        [Comment("负责人")]
        public long DirectorId { get; set; }


        [Comment("创建时间")]
        public DateTime CreateTime { get; set; }


        [Comment("修改时间")]
        public DateTime UpdateTime { get; set; }

        [Comment("创建人")]
        public long CreatorId { get; set; }

        [Comment("排序")]
        public int Sort { get; set; }

        [Comment("软删除")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 多对多配置关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SsuProjects> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {

        }
    }

    [Table("ssu_project_users")]
    [Comment("项目人员关联表")]
    public class SsuProjectUsers : IEntity, IEntityTypeBuilder<SsuProjectUsers, MasterDbContextLocator>
    {
        [Comment("项目编号")]
        [Key]
        public long ProjectId { get; set; }

        [Comment("参与人员")]
        //[Key]
        public long EmployeeId { get; set; }

        /// <summary>
        /// 多对多配置关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SsuProjectUsers> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {

        }
    }
}
