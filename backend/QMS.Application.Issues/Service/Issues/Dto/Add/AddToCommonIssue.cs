using MiniExcelLibs.Attributes;

namespace QMS.Application.Issues.Service.Issue.Dto.Add
{
    public class AddToCommonIssue
    {
        [ExcelColumnName("标题")]
        public virtual string Title { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        [ExcelColumnName("项目编号")]
        public virtual long ProjectId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [ExcelColumnName("产品编号")]
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 问题模块
        /// </summary>
        [ExcelColumnName("问题模块")]
        public virtual QMS.Core.Enum.EnumModule Module { get; set; }

        /// <summary>
        /// 问题性质
        /// </summary>
        [ExcelColumnName("问题性质")]
        public virtual QMS.Core.Enum.EnumConsequence Consequence { get; set; }

        /// <summary>
        /// 问题分类
        /// </summary>
        [ExcelColumnName("问题分类")]
        public virtual QMS.Core.Enum.EnumIssueClassification IssueClassification { get; set; }

        /// <summary>
        /// 分发人
        /// </summary>
        [ExcelColumnName("分发人编号")]
        public virtual long Dispatcher { get; set; }

        /// <summary>
        /// 问题来源
        /// </summary>
        [ExcelColumnName("问题来源")]
        public virtual QMS.Core.Enum.EnumIssueSource Source { get; set; }

        /// <summary>
        /// 发现人
        /// </summary>
        [ExcelColumnName("发现人编号")]
        public virtual long? Discover { get; set; }

        /// <summary>
        /// 发现日期
        /// </summary>
        [ExcelColumnName("发现日期")]
        public virtual DateTime? DiscoverTime { get; set; }

        /// <summary>
        /// 问题状态，用于Excel批量导入数据
        /// 界面添加可忽略该字段
        /// </summary>
        [ExcelIgnore]
        public virtual QMS.Core.Enum.EnumIssueStatus? Status { get; set; }

        /// <summary>
        /// 提出人，用于Excel批量导入数据
        /// 界面添加可忽略该字段
        /// </summary>
        [ExcelColumnName("提出人编号")]
        public virtual long? CreatorId { get; set; }

        /// <summary>
        /// 提出日期，用于Excel批量导入数据
        /// 界面添加可忽略该字段
        /// </summary>
        [ExcelColumnName("提出日期")]
        public virtual DateTime? CreateTime { get; set; }

        /// <summary>
        /// 被抄送人
        /// </summary>
        [ExcelColumnName("被抄送人编号")]
        public virtual long? CC { get; set; }
    }
}
