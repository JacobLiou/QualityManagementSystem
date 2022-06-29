using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furion.Extras.Admin.NET.Entity.Common
{
    /// <summary>
    /// 项目表
    /// </summary>
    [Table("ssu_project")]
    [Comment("项目表")]
    public class SsuProject : DEntityTenant, IEntityTypeBuilder<SsuProject, MasterDbContextLocator>
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Comment("项目编号")]
        //public int Id { get; set; }

        [Comment("项目名称")]
        [MaxLength(200)]
        [Required]
        public string ProjectName { get; set; }

        //[Comment("项目编号")]
        //[MaxLength(200)]
        //[Required]
        //public string ProjectCode { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        [Comment("负责人")]
        public long? DirectorId { get; set; }

        //[Comment("创建人")]
        //public long CreatorId { get; set; }

        //[Comment("创建时间")]
        //public DateTime CreateTime { get; set; }


        //[Comment("修改时间")]
        //public DateTime UpdateTime { get; set; }

        //[Comment("修改人")]
        //public long UpdateId { get; set; }

        [Comment("排序")]
        public int Sort { get; set; }

        //[Comment("软删除")]
        //public bool IsDeleted { get; set; }

        /// <summary>
        /// 项目描述
        /// </summary>
        [Comment("项目描述")]
        [MaxLength(2000)]
        public virtual string Description { get; set; }

        #region 项目、产品、人员关联

        [NotMapped]
        public ICollection<SsuProduct> SsuProducts { get; set; }

        [NotMapped]
        public List<SsuProjectProduct> SsuProjectsProducts { get; set; }

        [NotMapped]
        public ICollection<SysEmp> SysEmps { get; set; }

        [NotMapped]
        public List<SsuProjectUser> SsuProjectUsers { get; set; }

        #endregion 项目、产品、人员关联

        /// <summary>
        /// 多对多配置关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SsuProject> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
        }
    }

    [Table("ssu_project_user")]
    [Comment("项目人员关联表")]
    public class SsuProjectUser : IEntity
    {
        [Comment("项目编号")]
        public long ProjectId { get; set; }

        public SsuProject Project { get; set; }

        [Comment("参与人员")]
        public long EmployeeId { get; set; }

        public SysEmp Employee { get; set; }
    }
}