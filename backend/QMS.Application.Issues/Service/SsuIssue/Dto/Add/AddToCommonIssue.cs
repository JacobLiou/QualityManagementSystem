namespace QMS.Application.Issues.Service.SsuIssue.Dto.Add
{
    public class AddToCommonIssue
    {
        public virtual string Title { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public virtual long ProjectId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 问题模块
        /// </summary>
        public virtual QMS.Core.Enum.EnumModule Module { get; set; }

        /// <summary>
        /// 问题性质
        /// </summary>
        public virtual QMS.Core.Enum.EnumConsequence Consequence { get; set; }

        /// <summary>
        /// 问题分类
        /// </summary>
        public virtual QMS.Core.Enum.EnumIssueClassification IssueClassification { get; set; }

        /// <summary>
        /// 分发人
        /// </summary>
        public virtual long Dispatcher { get; set; }

        /// <summary>
        /// 问题来源
        /// </summary>
        public virtual QMS.Core.Enum.EnumIssueSource Source { get; set; }

        /// <summary>
        /// 发现人
        /// </summary>
        public virtual long Discover { get; set; }

        /// <summary>
        /// 发现日期
        /// </summary>
        public virtual DateTime DiscoverTime { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public virtual QMS.Core.Enum.EnumIssueStatus Status { get; set; }

        /// <summary>
        /// 提出人
        /// </summary>
        public virtual long CreatorId { get; set; }

        /// <summary>
        /// 提出日期
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 被抄送人
        /// </summary>
        public virtual long CC { get; set; }
    }
}
