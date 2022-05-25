using Furion.Extras.Admin.NET.Service;
using Furion.JsonSerialization;
using QMS.Application.Issues.Service.Issue.Dto.Update;
using QMS.Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题记录输入参数
    /// </summary>
    public class IssueInput //: PageInputBase
    {
        /// <summary>
        /// 问题简述
        /// </summary>
        [Required]
        public virtual string Title { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        [Required]
        public virtual long ProjectId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        //[Required]
        public virtual long? ProductId { get; set; }

        /// <summary>
        /// 问题模块
        /// </summary>
        [Required]
        public virtual Core.Enum.EnumModule Module { get; set; }

        /// <summary>
        /// 问题性质
        /// </summary>
        [Required]
        public virtual Core.Enum.EnumConsequence Consequence { get; set; }

        /// <summary>
        /// 问题分类
        /// </summary>
        [Required]
        public virtual Core.Enum.EnumIssueClassification IssueClassification { get; set; }

        /// <summary>
        /// 问题来源
        /// </summary>
        //[Required]
        public virtual Core.Enum.EnumIssueSource? Source { get; set; }

        ///// <summary>
        ///// 问题状态
        ///// 理论上不允许手动修改
        ///// </summary>
        //public virtual Core.Enum.EnumIssueStatus? Status { get; set; }

        /// <summary>
        /// 提出人
        /// </summary>
        //public virtual long? CreatorId { get; set; }

        /// <summary>
        /// 提出日期
        /// 理论上不允许手动改
        /// </summary>
        //public virtual DateTime? CreateTime { get; set; }

        /// <summary>
        /// 关闭日期
        /// 理论上不允许手动改
        /// </summary>
        //public virtual DateTime? CloseTime { get; set; }

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
        /// 理论上不允许手动改
        /// </summary>
        //public virtual long? Dispatcher { get; set; }

        /// <summary>
        /// 分发日期
        /// 理论上不允许手动改
        /// </summary>
        //public virtual DateTime? DispatchTime { get; set; }

        /// <summary>
        /// 预计完成日期
        /// 理论上不允许手动改
        /// </summary>
        //public virtual DateTime? ForecastSolveTime { get; set; }

        /// <summary>
        /// 被抄送人，可选多个
        /// </summary>
        public virtual List<long> CCList { get; set; } = new List<long>();

        /// <summary>
        /// 解决人
        /// 理论上不允许手动改
        /// </summary>
        //public virtual long? Executor { get; set; }

        /// <summary>
        /// 解决日期
        /// 理论上不允许手动改
        /// </summary>
        //public virtual DateTime? SolveTime { get; set; }

        /// <summary>
        /// 验证人，理论上提出人为验证人
        /// </summary>
        //public virtual long? Verifier { get; set; }

        /// <summary>
        /// 验证地点
        /// </summary>
        //public virtual string VerifierPlace { get; set; }

        /// <summary>
        /// 验证日期
        /// </summary>
        //public virtual DateTime? ValidateTime { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        //public virtual string Reason { get; set; }
        /// <summary>
        /// 解决措施
        /// </summary>
        //public virtual string Measures { get; set; }
        /// <summary>
        /// 验证情况
        /// </summary>
        //public virtual string Result { get; set; }
    }

    //public class AddIssueInput : IssueInput
    //{
    //    /// <summary>
    //    /// 问题简述
    //    /// </summary>
    //    [Required(ErrorMessage = "问题简述不能为空")]
    //    public override string Title { get; set; }

    //    /// <summary>
    //    /// 项目编号
    //    /// </summary>
    //    [Required(ErrorMessage = "项目编号不能为空")]
    //    public override long ProjectId { get; set; }

    //    /// <summary>
    //    /// 产品编号
    //    /// </summary>
    //    //[Required(ErrorMessage = "产品编号不能为空")]
    //    public override long? ProductId { get; set; }

    //    /// <summary>
    //    /// 问题模块
    //    /// </summary>
    //    [Required(ErrorMessage = "问题模块不能为空")]
    //    public override Core.Enum.EnumModule Module { get; set; }

    //    /// <summary>
    //    /// 问题性质
    //    /// </summary>
    //    [Required(ErrorMessage = "问题性质不能为空")]
    //    public override Core.Enum.EnumConsequence Consequence { get; set; }

    //    /// <summary>
    //    /// 问题分类
    //    /// </summary>
    //    [Required(ErrorMessage = "问题分类不能为空")]
    //    public override Core.Enum.EnumIssueClassification IssueClassification { get; set; }

    //    /// <summary>
    //    /// 问题来源
    //    /// </summary>
    //    [Required(ErrorMessage = "问题来源不能为空")]
    //    public override Core.Enum.EnumIssueSource Source { get; set; }

    //}

    public class DeleteIssueInput : BaseId
    {
    }

    public class UpdateIssueInput : IssueInput, IInput
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        [Required(ErrorMessage = "问题编号不能为空")]
        public long Id { get; set; }

        /// <summary>
        /// 当前问题指派给
        /// </summary>
        public long? CurrentAssignment { get; set; }

        public bool SetIssue(Issue issue)
        {
            issue.ProjectId = this.ProjectId > 0 ? this.ProjectId : issue.ProjectId;
            issue.ProductId = this.ProductId;
            issue.Title = this.Title;
            issue.Module = this.Module;
            issue.Consequence = this.Consequence;
            issue.IssueClassification = this.IssueClassification;

            issue.CurrentAssignment = this.CurrentAssignment;

            issue.Source = this.Source;
            issue.Discover = this.Discover;
            issue.DiscoverTime = this.DiscoverTime;

            //issue.Dispatcher = this.Dispatcher;

            //issue.ForecastSolveTime = this.ForecastSolveTime;
            if (this.CCList != null)
            {
                issue.CCs = JSON.Serialize(this.CCList);
            }
            //issue.Executor = this.Executor;
            //issue.SolveTime = this.SolveTime;
            //issue.ValidateTime = this.ValidateTime;
            //issue.Verifier = this.Verifier;
            //issue.VerifierPlace = this.VerifierPlace;

            return true;
        }

        public bool SetIssueDetail(IssueDetail issueDetail)
        {
            if (issueDetail.Description != issueDetail.Description)
            {
                issueDetail.Description = this.Description;
                return true;
            }
            //issueDetail.Reason = this.Reason;
            //issueDetail.Measures = this.Measures;
            //issueDetail.Result = this.Result;

            return false;
        }
    }

    public class QueryeIssueInput : BaseId
    {

    }
}
