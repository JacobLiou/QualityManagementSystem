using System;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题记录输出参数
    /// </summary>
    public class SsuIssueOutput
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 问题简述
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 项目编号
        /// </summary>
        public long ProjectId { get; set; }
        
        /// <summary>
        /// 产品编号
        /// </summary>
        public long ProductId { get; set; }
        
        /// <summary>
        /// 问题模块
        /// </summary>
        public QMS.Core.Enum.EnumModule Module { get; set; }
        
        /// <summary>
        /// 问题性质
        /// </summary>
        public QMS.Core.Enum.EnumConsequence Consequence { get; set; }
        
        /// <summary>
        /// 问题分类
        /// </summary>
        public QMS.Core.Enum.EnumIssueClassification IssueClassification { get; set; }
        
        /// <summary>
        /// 问题来源
        /// </summary>
        public QMS.Core.Enum.EnumIssueSource Source { get; set; }
        
        /// <summary>
        /// 问题状态
        /// </summary>
        public QMS.Core.Enum.EnumIssueStatus Status { get; set; }
        
        /// <summary>
        /// 提出人
        /// </summary>
        public long CreatorId { get; set; }
        
        /// <summary>
        /// 提出日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 关闭日期
        /// </summary>
        public DateTime CloseTime { get; set; }
        
        /// <summary>
        /// 发现人
        /// </summary>
        public long Discover { get; set; }
        
        /// <summary>
        /// 发现日期
        /// </summary>
        public DateTime DiscoverTime { get; set; }
        
        /// <summary>
        /// 分发人
        /// </summary>
        public long Dispatcher { get; set; }
        
        /// <summary>
        /// 分发日期
        /// </summary>
        public DateTime DispatchTime { get; set; }
        
        /// <summary>
        /// 预计完成日期
        /// </summary>
        public DateTime ForecastSolveTime { get; set; }
        
        /// <summary>
        /// 被抄送人
        /// </summary>
        public long CC { get; set; }
        
        /// <summary>
        /// 解决人
        /// </summary>
        public long Executor { get; set; }
        
        /// <summary>
        /// 解决日期
        /// </summary>
        public DateTime SolveTime { get; set; }
        
        /// <summary>
        /// 验证人
        /// </summary>
        public long Verifier { get; set; }
        
        /// <summary>
        /// 验证地点
        /// </summary>
        public string VerifierPlace { get; set; }
        
        /// <summary>
        /// 验证日期
        /// </summary>
        public DateTime ValidateTime { get; set; }
        
    }
}
