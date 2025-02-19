﻿using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{
    [Table("issue_ext_attr")]
    [Comment("问题扩展属性")]
    public class IssueExtendAttribute :DEntityTenant, IEntity<IssuesDbContextLocator>, IEntityTypeBuilder<IssueExtendAttribute, IssuesDbContextLocator>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("字段编号")]
        public override long Id { get; set; }

        [Comment("模块编号")]
        public EnumModule Module { get; set; }

        [Comment("字段名")]
        [MaxLength(50)]
        [Required]
        public string AttibuteName { get; set; }

        [Comment("字段代码")]
        [MaxLength(50)]
        public string AttributeCode { get; set; }

        [Comment("字段值类型")]
        [MaxLength(30)]
        [Required]
        public string ValueType { get; set; }

        [Comment("创建人")]
        public long CreatorId { get; set; }


        [Comment("创建时间")]
        public DateTime CreateTime { get; set; }

        [Comment("更新人")]
        public long UpdateId { get; set; }

        [Comment("提出日期")]
        public DateTime UpdateTime { get; set; }

        public void SetCreate()
        {
            this.CreatorId = CurrentUserInfo.UserId;
            this.UpdateId = this.CreatorId;
            this.CreateTime = DateTime.Now;
            this.UpdateTime = this.CreateTime;
        }

        public void SetUpdate()
        {
            this.UpdateId = CurrentUserInfo.UserId;
            this.UpdateTime = DateTime.Now;
        }

        public void SetDelete()
        {
            this.SetUpdate();
            this.IsDeleted = true;
        }

        [NotMapped]
        public ICollection<Issue> Issues { get; set; }

        [NotMapped]
        public ICollection<IssueExtendAttributeValue> AttrValues { get; set; }

        public void Configure(EntityTypeBuilder<IssueExtendAttribute> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            //entityBuilder.HasMany(i => i.AttrValues)
            //    .WithOne(i => i.IssueExtendAttribute)
            //    .HasForeignKey(i => i.Id);

            // 设置联合主键和外键
            entityBuilder
                .HasMany(model => model.Issues)
                .WithMany(model => model.Attrs)
                .UsingEntity<IssueExtendAttributeValue>(
                u => u.HasOne(c => c.Issue).WithMany(c => c.AttrValues).HasForeignKey(c => c.IssueNum),
                u => u.HasOne(c => c.IssueExtendAttribute).WithMany(c => c.AttrValues).HasForeignKey(c => c.Id),
                u =>
                {
                    u.HasKey(c => new { c.IssueNum, c.Id });
                });
        }

        [Comment("排序优先级")]
        public int Sort { get; set; }

        [Comment("软删除")]
        public bool IsDeleted { get; set; } = false;
    }
}
