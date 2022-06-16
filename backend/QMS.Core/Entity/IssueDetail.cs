using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{
    [Table("issue_detail")]
    [Comment("详细问题记录")]
    public class IssueDetail : IEntity<IssuesDbContextLocator>, IEntityTypeBuilder<IssueDetail, IssuesDbContextLocator>
    {
        [Key]
        [Comment("问题编号")]
        public long Id { get; set; }

        [ForeignKey(nameof(Id))]
        [NotMapped]
        public Issue Issue { get; set; }

        [Comment("问题详情")]
        public string Description { get; set; }

        [Comment("原因分析")]
        public string Reason { get; set; }

        [Comment("解决措施")]
        public string Measures { get; set; }

        [Comment("验证数量")]
        public int? Count { get; set; }

        [Comment("验证批次")]
        [MaxLength(100)]
        public string Batch { get; set; }

        [Comment("验证情况")]
        public string Result { get; set; }

        [Comment("复核情况")]
        public string ReCheckResult { get; set; }

        [Comment("解决版本")]
        [MaxLength(200)]
        public string SolveVersion { get; set; }

        [Comment("备注")]
        public string Comment { get; set; }

        [Comment("挂起情况")]
        public string HangupReason { get; set; }

        [Comment("关闭原因")]
        public string CloseReason { get; set; }

        /// <summary>
        /// 用于新增和分发时保存动态生成的字段信息（动态生成对应控件时,字段结构可通过相应接口获得）
        /// issueId：问题编号 long
        /// attributeId：字段编号  long
        /// attributeCode：字段代码 string
        /// value：字段值 string
        /// valueType：字段类型 string
        ///
        /// [{"issueId":284932473958469,"attributeId":285613677277253,"attributeCode":"code","value":"数据","valueType":"string"}]
        /// </summary>
        [Comment("扩展属性")]
        public string ExtendAttribute { get; set; }

        /// <summary>
        /// 用于查询该问题有哪些附件
        /// [{"AttachmentId":287372965613637,"FileName":"20220511164546.xlsx","AttachmentType":0}]
        /// </summary>
        [Comment("附件信息")]
        public string Attachments { get; set; }

        public void Configure(EntityTypeBuilder<IssueDetail> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
        }
    }
}