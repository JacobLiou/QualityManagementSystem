using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET.Entity.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furion.Extras.Admin.NET
{
    /// <summary>
    /// 员工表
    /// </summary>
    [Table("sys_emp")]
    [Comment("员工表")]
    public class SysEmp : IEntity, IEntityTypeBuilder<SysEmp>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("用户Id")]
        public long Id { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Comment("工号")]
        [MaxLength(30)]
        public string JobNum { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        [Comment("机构Id")]
        public long OrgId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Comment("机构名称")]
        [MaxLength(50)]
        public string OrgName { get; set; }

        /// <summary>
        /// 多对多（职位）
        /// </summary>
        public ICollection<SysPos> SysPos { get; set; }

        /// <summary>
        /// 多对多中间表（员工-职位）
        /// </summary>
        public List<SysEmpPos> SysEmpPos { get; set; }

        /// <summary>
        /// 多对多配置关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SysEmp> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            entityBuilder.HasMany(p => p.SysPos).WithMany(p => p.SysEmps).UsingEntity<SysEmpPos>(
                u => u.HasOne(c => c.SysPos).WithMany(c => c.SysEmpPos).HasForeignKey(c => c.SysPosId),
                u => u.HasOne(c => c.SysEmp).WithMany(c => c.SysEmpPos).HasForeignKey(c => c.SysEmpId),
                u =>
                {
                    u.HasKey(c => new { c.SysEmpId, c.SysPosId });
                });

            // 项目、人员
            entityBuilder.HasMany(p => p.SsuProjects).WithMany(p => p.SysEmps).UsingEntity<SsuProjectUser>(
                u => u.HasOne(c => c.Project).WithMany(c => c.SsuProjectUsers).HasForeignKey(c => c.ProjectId),
                u => u.HasOne(c => c.Employee).WithMany(c => c.SsuProjectUsers).HasForeignKey(c => c.EmployeeId),
                u =>
                {
                    u.HasKey(c => new { c.ProjectId, c.EmployeeId });
                });

            // 产品、人员
            entityBuilder.HasMany(p => p.SsuProducts).WithMany(p => p.SysEmps).UsingEntity<SsuProductUser>(
                u => u.HasOne(c => c.Product).WithMany(c => c.SsuProductUsers).HasForeignKey(c => c.ProductId),
                u => u.HasOne(c => c.Employee).WithMany(c => c.SsuProductUsers).HasForeignKey(c => c.EmployeeId),
                u =>
                {
                    u.HasKey(c => new { c.ProductId, c.EmployeeId });
                });

            // 人员组、成员
            entityBuilder.HasMany(p => p.SsuGroups).WithMany(p => p.SysEmps).UsingEntity<SsuGroupUser>(
                u => u.HasOne(c => c.Group).WithMany(c => c.SsuGroupUsers).HasForeignKey(c => c.GroupId),
                u => u.HasOne(c => c.Employee).WithMany(c => c.SsuGroupUsers).HasForeignKey(c => c.EmployeeId),
                u =>
                {
                    u.HasKey(c => new { c.GroupId, c.EmployeeId });
                });
        }


        // ****************************************************************************************
        #region 产品、项目、分组与人员关联
        public ICollection<SsuProduct> SsuProducts { get; set; }

        public List<SsuProductUser> SsuProductUsers { get; set; }


        public ICollection<SsuProject> SsuProjects { get; set; }

        public List<SsuProjectUser> SsuProjectUsers { get; set; }

        public ICollection<SsuGroup> SsuGroups { get; set; }

        public List<SsuGroupUser> SsuGroupUsers { get; set; }
        #endregion
    }
}