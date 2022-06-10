using MiniExcelLibs.Attributes;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issue.Dto.Add
{
    public class AddToCommonIssue
    {
        /// <summary>
        /// 问题简述
        /// </summary>
        [ExcelColumnName("标题")]
        [ExcelColumnIndex(0)]
        [ExcelColumnWidth(30)]
        [Required]
        public virtual string Title { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        [ExcelColumnName("项目编号")]
        [ExcelColumnWidth(20)]
        [Required]
        public virtual long ProjectId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [ExcelColumnName("产品编号")]
        [ExcelColumnWidth(20)]
        public virtual long? ProductId { get; set; }

        /// <summary>
        /// 问题模块
        /// </summary>
        [ExcelColumnName("问题模块")]
        [ExcelColumnWidth(20)]
        [Required]
        public virtual Core.Enum.EnumModule Module { get; set; }

        /// <summary>
        /// 问题分类
        /// </summary>
        [ExcelColumnName("问题分类")]
        [ExcelColumnWidth(20)]
        [Required]
        public virtual Core.Enum.EnumIssueClassification IssueClassification { get; set; }

        /// <summary>
        /// 分发人
        /// </summary>
        /// 新增时数据库不存储分发人数据，先暂时保留这个字段，但是将改字段设置成非必录，后续再删除
        [ExcelColumnName("分发人编号")]
        [ExcelColumnWidth(20)]
        public virtual long? Dispatcher { get; set; }

        /// <summary>
        /// 当前指派给
        /// </summary>
        [ExcelColumnName("当前指派给")]
        [ExcelColumnWidth(20)]
        [Required]
        public long? CurrentAssignment { get; set; }

        /// <summary>
        /// 问题性质
        /// </summary>
        [ExcelColumnName("问题性质")]
        [ExcelColumnWidth(20)]
        [Required]
        public virtual Core.Enum.EnumConsequence Consequence { get; set; }

        /// <summary>
        /// 问题来源
        /// </summary>
        [ExcelColumnName("问题来源")]
        [ExcelColumnWidth(20)]
        public virtual Core.Enum.EnumIssueSource? Source { get; set; }

        /// <summary>
        /// 发现人
        /// </summary>
        [ExcelColumnName("发现人编号")]
        [ExcelColumnWidth(20)]
        public virtual long? Discover { get; set; }

        /// <summary>
        /// 发现日期
        /// </summary>
        [ExcelColumnName("发现日期")]
        [ExcelColumnWidth(20)]
        public virtual DateTime? DiscoverTime { get; set; }

        /// <summary>
        /// 问题状态，用于Excel批量导入数据
        /// 界面添加可忽略该字段
        /// </summary>
        [ExcelIgnore]
        public virtual Core.Enum.EnumIssueStatus? Status { get; set; }

        /// <summary>
        /// 提出人，用于Excel批量导入数据
        /// 界面添加可忽略该字段
        /// </summary>
        [ExcelIgnore]
        public virtual long? CreatorId { get; set; }

        /// <summary>
        /// 提出日期，用于Excel批量导入数据
        /// 界面添加可忽略该字段
        /// </summary>
        [ExcelIgnore]
        public virtual DateTime? CreateTime { get; set; }

        /// <summary>
        /// 被抄送人，可选多个
        /// </summary>
        [ExcelIgnore]
        public virtual List<long> CCList { get; set; } = new List<long>();

        /// <summary>
        /// 是否是暂存
        /// </summary>
        [ExcelIgnore]
        public virtual bool IsTemporary { get; set; }
    }
}