using Furion.Extras.Admin.NET.Service;
using Furion.JsonSerialization;
using QMS.Application.Issues.Helper;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issue.Dto.Query
{
    public class OutputGeneralIssue : BaseId
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 项目名
        /// </summary>
        [Required]
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        [Required]
        public long ProjectId { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        //[Required]
        public long? ProductId { get; set; }

        /// <summary>
        /// 模块名
        /// </summary>
        [Required]
        public EnumModule Module { get; set; }

        /// <summary>
        /// 问题性质
        /// </summary>
        [Required]
        public EnumConsequence Consequence { get; set; }

        /// <summary>
        /// 问题分类
        /// </summary>
        [Required]
        public EnumIssueClassification IssueClassification { get; set; }

        /// <summary>
        /// 问题来源
        /// </summary>
        //[Required]
        public EnumIssueSource? Source { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        [Required]
        public EnumIssueStatus Status { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>
        [Required]
        public long Creator { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Required]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseTime { get; set; }

        /// <summary>
        /// 发现人编号
        /// </summary>
        public long? Discover { get; set; }

        /// <summary>
        /// 发现人名称
        /// </summary>
        public string DiscoverName { get; set; }

        /// <summary>
        /// 发现时间
        /// </summary>
        public DateTime? DiscoverTime { get; set; }

        /// <summary>
        /// 分发人
        /// </summary>
        public long? Dispatcher { get; set; }

        /// <summary>
        /// 分发人名称
        /// </summary>
        public string DispatcherName { get; set; }

        /// <summary>
        /// 分发时间
        /// </summary>
        public DateTime? DispatchTime { get; set; }

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime? ForecastSolveTime { get; set; }

        /// <summary>
        /// 被抄送人ID列表
        /// </summary>
        public List<long> CopyTo { get; set; }

        /// <summary>
        /// 被抄送人名称列表
        /// </summary>
        public List<string> CopyToName { get; set; }

        /// <summary>
        /// 处理人ID
        /// </summary>
        public long? Executor { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string ExecutorName { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? SolveTime { get; set; }

        /// <summary>
        /// 验证人ID
        /// </summary>
        public long? Verifier { get; set; }

        /// <summary>
        /// 验证人名称
        /// </summary>
        public string VerifierName { get; set; }

        /// <summary>
        /// 验证地点
        /// </summary>
        public string VerifierPlace { get; set; }

        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime? ValidateTime { get; set; }

        /// <summary>
        /// 回归验证状态
        /// 0: 没有走到验证  1: 验证不通过 2: 验证通过
        /// </summary>
        public int ValidationStatus { get; set; }

        /// <summary>
        /// 当前指派人ID
        /// </summary>
        public long? CurrentAssignId { get; set; }

        /// <summary>
        /// 当前指派人名称
        /// </summary>
        public string CurrentAssignName { get; set; }

        /// <summary>
        /// 可显示操作按钮列表
        /// </summary>
        public List<EnumIssueButton> BtnList { get; set; }

        /// <summary>
        /// 问题序号（格式为：模块缩写+时间年月日+三位数自增种子，如TST20220620002）
        /// </summary>
        public string SerialNumber { get; set; }

        public OutputGeneralIssue(Core.Entity.Issue model)
        {
            this.Id = model.Id;
            this.Title = model.Title;
            this.Module = model.Module;
            this.Consequence = model.Consequence;
            this.IssueClassification = model.IssueClassification;
            this.Source = model.Source;
            this.Status = model.Status;
            this.CreateTime = model.CreateTime;
            this.DiscoverTime = model.DiscoverTime;
            this.DispatchTime = model.DispatchTime;
            this.ForecastSolveTime = model.ForecastSolveTime;
            this.SolveTime = model.SolveTime;
            this.VerifierPlace = model.VerifierPlace;
            this.ValidateTime = model.ValidateTime;

            this.Creator = model.CreatorId;
            this.CreatorName = model.CreatorId.GetNameByEmpId();

            this.Dispatcher = model.Dispatcher;
            this.DispatcherName = model.Dispatcher.GetNameByEmpId();

            this.ProjectName = model.ProjectId.GetNameByProjectId();
            this.ProjectId = model.ProjectId;

            this.ProductId = model.ProductId;
            if (this.ProductId != null)
            {
                this.ProductName = ((long)model.ProductId).GetNameByProductId();
            }

            this.Discover = model.Discover;
            this.DiscoverName = model.Discover.GetNameByEmpId();

            this.Executor = model.Executor;
            this.ExecutorName = model.Executor.GetNameByEmpId();

            if (model.Verifier != null)
            {
                this.Verifier = model.Verifier;
                this.VerifierName = model.Verifier.GetNameByEmpId();
            }

            this.CurrentAssignId = model.CurrentAssignment;
            this.CurrentAssignName = model.CurrentAssignment.GetNameByEmpId();

            this.CloseTime = model.CloseTime;

            this.ValidationStatus = model.ValidationStatus;

            if (!string.IsNullOrEmpty(model.CCs))
            {
                this.CopyTo = JSON.Deserialize<List<long>>(model.CCs);
                this.CopyToName = this.CopyTo?.Select<long, string>(id => id.GetNameByEmpId())?.ToList();
            }
            this.SerialNumber = model.SerialNumber;
        }
    }
}