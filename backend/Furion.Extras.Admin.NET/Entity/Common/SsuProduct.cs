using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET.Entity.Common.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furion.Extras.Admin.NET.Entity.Common
{
    /// <summary>
    /// 产品表
    /// </summary>
    [Table("ssu_product")]
    [Comment("产品表")]
    public class SsuProduct : DEntityTenant, IEntityTypeBuilder<SsuProduct, MasterDbContextLocator>
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Comment("产品编号")]
        //public long Id { get; set; }

        [Comment("产品名称")]
        [MaxLength(200)]
        [Required]
        public string ProductName { get; set; }

        [Comment("产品型号")]
        [MaxLength(100)]
        [Required]
        public string ProductType { get; set; }

        //[Comment("产品线")]
        //[MaxLength(100)]
        //[Required]
        //public EnumProductLine ProductLine { get; set; }


        //[Comment("所属项目")]
        //[MaxLength(100)]
        //public long ProjectId { get; set; }

        //[ForeignKey(nameof(ProjectId))]
        //[NotMapped]
        //public SsuProject Project { get; set; }

        [Comment("状态")]
        [MaxLength(100)]
        public EnumProductStatus Status { get; set; }


        [Comment("产品分类")]
        public EnumProductClassfication ClassificationId { get; set; }


        //[Comment("创建时间")]
        //public DateTime CreateTime { get; set; }

        //[Comment("创建人")]
        //public long CreatorId { get; set; }

        //[Comment("修改时间")]
        //public DateTime UpdateTime { get; set; }

        //[Comment("修改人")]
        //public long UpdateId { get; set; }

        //[Comment("排序")]
        //public int Sort { get; set; }

        //[Comment("软删除")]
        //public bool IsDeleted { get; set; }

        [Comment("负责人")]
        public long DirectorId { get; set; }

        //************************************************************************

        #region 关联

        [NotMapped]
        public ICollection<SsuProject> SsuProjects { get; set; }

        [NotMapped]
        public List<SsuProjectProduct> SsuProjectsProducts { get; set; }

        [NotMapped]
        public ICollection<SysEmp> SysEmps { get; set; }

        [NotMapped]
        public List<SsuProductUser> SsuProductUsers { get; set; }

        #endregion 关联

        public void Configure(EntityTypeBuilder<SsuProduct> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            // 项目、产品关联
            entityBuilder.HasMany(p => p.SsuProjects).WithMany(p => p.SsuProducts).UsingEntity<SsuProjectProduct>(
                u => u.HasOne(c => c.Project).WithMany(c => c.SsuProjectsProducts).HasForeignKey(c => c.ProjectId),
                u => u.HasOne(c => c.Product).WithMany(c => c.SsuProjectsProducts).HasForeignKey(c => c.ProductId),
                u =>
                {
                    u.HasKey(c => new { c.ProjectId, c.ProductId });
                });
        }
    }

    [Table("ssu_project_product")]
    [Comment("项目产品关联表")]
    public class SsuProjectProduct : IEntity
    {
        [Comment("项目编号")]
        public long ProjectId { get; set; }

        public SsuProject Project { get; set; }

        [Comment("产品编号")]
        public long ProductId { get; set; }

        public SsuProduct Product { get; set; }
    }

    [Table("ssu_product_user")]
    [Comment("产品人员关联表")]
    public class SsuProductUser : IEntity
    {
        [Comment("产品编号")]
        public long ProductId { get; set; }

        public SsuProduct Product { get; set; }

        [Comment("参与人员")]
        public long EmployeeId { get; set; }

        public SysEmp Employee { get; set; }
    }
}