using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{
    [Table("ssu_issue_extend_attribute")]
    [Comment("问题扩展属性")]
    public class SsuIssueExtendAttribute : IEntity<IssuesDbContextLocator>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("字段编号")]
        public long Id { get; set; }

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

        [Comment("排序优先级")]
        public int Sort { get; set; }

        [Comment("软删除")]
        public bool IsDeleted { get; set; } = false;
    }
}
