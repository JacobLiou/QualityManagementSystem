using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using QMS.Application.Issues.Service.SsuIssue.Dto.Update;
using QMS.Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题记录输入参数
    /// </summary>
    public class SsuIssueInput : PageInputBase
    {
        /// <summary>
        /// 问题简述
        /// </summary>
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
        /// 问题来源
        /// </summary>
        public virtual QMS.Core.Enum.EnumIssueSource Source { get; set; }

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
        /// 关闭日期
        /// </summary>
        public virtual DateTime? CloseTime { get; set; }

        /// <summary>
        /// 发现人
        /// </summary>
        public virtual long? Discover { get; set; }

        /// <summary>
        /// 发现日期
        /// </summary>
        public virtual DateTime? DiscoverTime { get; set; }

        /// <summary>
        /// 分发人
        /// </summary>
        public virtual long? Dispatcher { get; set; }

        /// <summary>
        /// 分发日期
        /// </summary>
        public virtual DateTime? DispatchTime { get; set; }

        /// <summary>
        /// 预计完成日期
        /// </summary>
        public virtual DateTime? ForecastSolveTime { get; set; }

        /// <summary>
        /// 被抄送人
        /// </summary>
        public virtual long? CC { get; set; }

        /// <summary>
        /// 解决人
        /// </summary>
        public virtual long? Executor { get; set; }

        /// <summary>
        /// 解决日期
        /// </summary>
        public virtual DateTime? SolveTime { get; set; }

        /// <summary>
        /// 验证人
        /// </summary>
        public virtual long? Verifier { get; set; }

        /// <summary>
        /// 验证地点
        /// </summary>
        public virtual string VerifierPlace { get; set; }

        /// <summary>
        /// 验证日期
        /// </summary>
        public virtual DateTime? ValidateTime { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public virtual string? Description { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public virtual string? Reason { get; set; }
        /// <summary>
        /// 解决措施
        /// </summary>
        public virtual string? Measures { get; set; }
        /// <summary>
        /// 验证情况
        /// </summary>
        public virtual string? Result { get; set; }
    }

    public class AddSsuIssueInput : SsuIssueInput
    {
        /// <summary>
        /// 问题简述
        /// </summary>
        [Required(ErrorMessage = "问题简述不能为空")]
        public override string Title { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        [Required(ErrorMessage = "项目编号不能为空")]
        public override long ProjectId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [Required(ErrorMessage = "产品编号不能为空")]
        public override long ProductId { get; set; }

        /// <summary>
        /// 问题模块
        /// </summary>
        [Required(ErrorMessage = "问题模块不能为空")]
        public override QMS.Core.Enum.EnumModule Module { get; set; }

        /// <summary>
        /// 问题性质
        /// </summary>
        [Required(ErrorMessage = "问题性质不能为空")]
        public override QMS.Core.Enum.EnumConsequence Consequence { get; set; }

        /// <summary>
        /// 问题分类
        /// </summary>
        [Required(ErrorMessage = "问题分类不能为空")]
        public override QMS.Core.Enum.EnumIssueClassification IssueClassification { get; set; }

        /// <summary>
        /// 问题来源
        /// </summary>
        [Required(ErrorMessage = "问题来源不能为空")]
        public override QMS.Core.Enum.EnumIssueSource Source { get; set; }

    }

    public class DeleteSsuIssueInput : BaseId
    {
    }

    public class UpdateSsuIssueInput : SsuIssueInput, IInput
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        [Required(ErrorMessage = "问题编号不能为空")]
        public long Id { get; set; }

        public bool SetIssue(SsuIssue issue)
        {
            issue.ProjectId = this.ProjectId > 0 ? this.ProjectId : issue.ProjectId;
            issue.ProductId = this.ProductId > 0 ? this.ProductId : issue.ProductId;
            issue.Title = this.Title;
            issue.Module = this.Module;
            issue.Consequence = this.Consequence;
            issue.IssueClassification = this.IssueClassification;
            issue.Source = this.Source;
            issue.Source = this.Source;
            issue.Discover = this.Discover;
            issue.DiscoverTime = this.DiscoverTime;
            issue.Dispatcher = this.Dispatcher;
            issue.ForecastSolveTime = this.ForecastSolveTime;
            issue.CC = this.CC;
            issue.Executor = this.Executor;
            issue.SolveTime = this.SolveTime;
            issue.ValidateTime = this.ValidateTime;
            issue.Verifier= this.Verifier;
            issue.VerifierPlace = this.VerifierPlace;

            return true;
        }

        public bool SetIssueDetail(SsuIssueDetail issueDetail)
        {
            issueDetail.Description = this.Description;
            issueDetail.Reason = this.Reason;
            issueDetail.Measures = this.Measures;
            issueDetail.Result = this.Result;

            return true;
        }
    }

    public class QueryeSsuIssueInput : BaseId
    {

    }
}
