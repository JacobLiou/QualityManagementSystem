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

namespace QMS.Core.Entity.common
{

    /// <summary>
    /// 产品表
    /// </summary>
    [Table("ssu_products")]
    [Comment("产品表")]
    public class SsuProducts : IEntity, IEntityTypeBuilder<SsuProducts, MasterDbContextLocator>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("产品编号")]
        public int Id { get; set; }

        [Comment("产品名称")]
        [MaxLength(200)]
        public string ProductName { get; set; }

        [Comment("产品类型")]
        [MaxLength(100)]
        public string ProductType { get; set; }

        [Comment("产品线")]
        [MaxLength(100)]
        public string ProductLine { get; set; }


        [Comment("所属项目")]
        [MaxLength(100)]
        public string ProjectId { get; set; }

        [Comment("状态")]
        [MaxLength(100)]
        public string Status { get; set; }


        [Comment("分类")]
        public long ClassificationId { get; set; }


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

        [Comment("负责人")]
        public long DirectorId { get; set; }

        /// <summary>
        /// 多对多配置关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SsuProducts> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {

        }
    }

    [Table("ssu_product_users")]
    [Comment("产品人员关联表")]
    public class SsuProductUsers : IEntity, IEntityTypeBuilder<SsuProductUsers, MasterDbContextLocator>
    {
        [Comment("产品编号")]
        [Key]
        public long ProductId { get; set; }

        [Comment("参与人员")]
        //[Key]
        public long EmployeeId { get; set; }

        /// <summary>
        /// 多对多配置关系
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void Configure(EntityTypeBuilder<SsuProductUsers> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {

        }
    }
}
