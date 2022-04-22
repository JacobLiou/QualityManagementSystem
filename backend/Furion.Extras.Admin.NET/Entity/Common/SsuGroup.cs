using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furion.Extras.Admin.NET.Entity.Common
{
    /// <summary>
    /// 人员组表
    /// </summary>
    [Table("ssu_group")]
    [Comment("人员组表")]
    public class SsuGroup : DEntityBase, IEntityTypeBuilder<SsuGroup, MasterDbContextLocator>
    {

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Comment("产品编号")]
        //public long Id { get; set; }

        [Comment("人员组名称")]
        [MaxLength(200)]
        [Required]
        public string GroupName { get; set; }


        //[Comment("创建时间")]
        //public DateTime CreateTime { get; set; }

        //[Comment("创建人")]
        //public long CreatorId { get; set; }

        //[Comment("修改时间")]
        //public DateTime UpdateTime { get; set; }

        //[Comment("修改人")]
        //public long UpdateId { get; set; }

        [Comment("排序")]
        public int Sort { get; set; }

        //[Comment("软删除")]
        //public bool IsDeleted { get; set; }

        //************************************************************************
        [NotMapped]
        public ICollection<SysEmp> SysEmps { get; set; }
        [NotMapped]
        public List<SsuGroupUser> SsuGroupUsers { get; set; }

        public void Configure(EntityTypeBuilder<SsuGroup> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
        }
    }

    [Table("ssu_group_user")]
    [Comment("人员组成员关联表")]
    public class SsuGroupUser : IEntity
    {
        [Comment("人员组编号")]
        public long GroupId { get; set; }

        public SsuGroup Group { get; set; }

        [Comment("成员编号")]
        public long EmployeeId { get; set; }

        public SysEmp Employee { get; set; }
    }
}
