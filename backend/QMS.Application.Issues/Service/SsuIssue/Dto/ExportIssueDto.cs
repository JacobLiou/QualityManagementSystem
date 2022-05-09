﻿using Microsoft.EntityFrameworkCore;
using MiniExcelLibs.Attributes;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;

namespace QMS.Application.Issues.Service.SsuIssue.Dto
{
    public class ExportIssueDto
    {
        [ExcelColumnName("问题编号")]
        [Comment("问题编号")]
        public long Id { get; set; }

        [ExcelColumnName("问题简述")]
        [Comment("问题简述")]
        public string Title { get; set; }

        [ExcelColumnName("项目名称")]
        [Comment("项目名称")]
        public string ProjectName { get; set; }

        [ExcelColumnName("产品名称")]
        [Comment("产品名称")]
        public string ProductName { get; set; }

        [ExcelColumnName("问题模块")]
        [Comment("问题模块")]
        public string Module { get; set; }

        [ExcelColumnName("问题性质")]
        [Comment("问题性质")]
        public string Consequence { get; set; }

        [ExcelColumnName("问题分类")]
        [Comment("问题分类")]
        public string IssueClassification { get; set; }

        [ExcelColumnName("问题来源")]
        [Comment("问题来源")]
        public string Source { get; set; }

        [ExcelColumnName("问题状态")]
        [Comment("问题状态")]
        public string Status { get; set; }

        [NotToTableColumnAttribute]
        [ExcelIgnore]
        public EnumIssueStatus IssueStatus { get; set; }

        [ExcelColumnName("提出人")]
        [Comment("提出人")]
        public string Creator { get; set; }

        [NotToTableColumnAttribute]
        [ExcelIgnore]
        public long? CreatorId { get; set; }

        [ExcelColumnName("提出日期")]
        [Comment("提出日期")]
        public string CreateTime { get; set; }

        [ExcelColumnName("关闭日期")]
        [Comment("关闭日期")]
        public string CloseTime { get; set; }

        [ExcelColumnName("发现人")]
        [Comment("发现人")]
        public string Discover { get; set; }

        [ExcelColumnName("发现日期")]
        [Comment("发现日期")]
        public string DiscoverTime { get; set; }

        [ExcelColumnName("分发人")]
        [Comment("分发人")]
        public string Dispatcher { get; set; }

        [NotToTableColumnAttribute]
        [ExcelIgnore]
        public long? DispatcherId { get; set; }

        [ExcelColumnName("分发日期")]
        [Comment("分发日期")]
        public string DispatchTime { get; set; }

        [ExcelColumnName("预计完成日期")]
        [Comment("预计完成日期")]
        public string ForecastSolveTime { get; set; }

        [ExcelColumnName("被抄送人")]
        [Comment("被抄送人")]
        public string CC { get; set; }

        [ExcelColumnName("解决人")]
        [Comment("解决人")]
        public string Executor { get; set; }

        [NotToTableColumnAttribute]
        [ExcelIgnore]
        public long? ExecutorId { get; set; }

        [ExcelColumnName("解决日期")]
        [Comment("解决日期")]
        public string SolveTime { get; set; }

        [ExcelColumnName("验证人")]
        [Comment("验证人")]
        public string Verifier { get; set; }

        [ExcelColumnName("验证地点")]
        [Comment("验证地点")]
        public string VerifierPlace { get; set; }

        [ExcelColumnName("验证日期")]
        [Comment("验证日期")]
        public string ValidateTime { get; set; }

        [ExcelColumnName("问题详情")]
        [Comment("问题详情")]
        public string Description { get; set; }

        [ExcelColumnName("原因分析")]
        [Comment("原因分析")]
        public string Reason { get; set; }

        [ExcelColumnName("解决措施")]
        [Comment("解决措施")]
        public string Measures { get; set; }

        [ExcelColumnName("验证数量")]
        [Comment("验证数量")]
        public int? Count { get; set; }

        [ExcelColumnName("验证批次")]
        [Comment("验证批次")]
        public string Batch { get; set; }

        [ExcelColumnName("验证情况")]
        [Comment("验证情况")]
        public string Result { get; set; }

        [ExcelColumnName("解决版本")]
        [Comment("解决版本")]
        public string SolveVersion { get; set; }

        [ExcelColumnName("备注")]
        [Comment("备注")]
        public string Comment { get; set; }

        [ExcelColumnName("挂起情况")]
        [Comment("挂起情况")]
        public string HangupReason { get; set; }

        [ExcelColumnName("扩展属性")]
        [Comment("扩展属性")]
        public string ExtendAttribute { get; set; }

        public ExportIssueDto(QMS.Core.Entity.SsuIssue issue, SsuIssueDetail detail)
        {
            this.Id = issue.Id;

            this.Title = issue.Title;
            this.Module = issue.Module.GetEnumDescription<EnumModule>();
            this.Consequence = issue.Consequence.GetEnumDescription<EnumConsequence>();
            this.IssueClassification = issue.IssueClassification.GetEnumDescription<EnumIssueClassification>();
            this.Source = issue.Source.GetEnumDescription<EnumIssueSource>();
            this.Status = issue.Status.GetEnumDescription<EnumIssueStatus>();
            this.CreateTime = issue.CreateTime.GetTimeString();
            this.DiscoverTime = issue.DiscoverTime.GetTimeString();
            this.DispatchTime = issue.DispatchTime.GetTimeString();
            this.ForecastSolveTime = issue.ForecastSolveTime.GetTimeString();
            this.SolveTime = issue.SolveTime.GetTimeString();
            this.ValidateTime = issue.ValidateTime.GetTimeString();
            this.VerifierPlace = issue.VerifierPlace;

            this.Creator = issue.CreatorId.GetNameByEmpId();
            this.Dispatcher = issue.Dispatcher.GetNameByEmpId();
            this.ProjectName = issue.ProjectId.GetNameByProjectId();
            this.ProductName = issue.ProductId.GetNameByProductId();
            this.Discover = issue.Discover.GetNameByEmpId();
            this.Executor = issue.Executor.GetNameByEmpId();
            this.Verifier = issue.Verifier == null ? this.Creator : issue.Verifier.GetNameByEmpId();
            

            this.CloseTime = issue.CloseTime.GetTimeString();
            this.CC = issue.CC.GetNameByEmpId();



            this.Description = detail.Description;
            this.Batch = detail.Batch;
            this.Count = detail.Count;
            this.SolveVersion = detail.SolveVersion;
            this.Comment = detail.Comment;
            this.ExtendAttribute = detail.ExtendAttribute;
            this.Result = detail.Result;

            this.Measures = detail.Measures;
            this.Reason = detail.Reason;
            this.HangupReason = detail.HangupReason;


            // **********************************
            this.IssueStatus = issue.Status;
            this.CreatorId = issue.CreatorId;
            this.DispatcherId = issue.Dispatcher;
            this.ExecutorId = issue.Executor;
        }
    }
}
