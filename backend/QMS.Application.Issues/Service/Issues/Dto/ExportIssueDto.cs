using Furion;
using Furion.JsonSerialization;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs.Attributes;
using QMS.Application.Issues.Field;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;

namespace QMS.Application.Issues.Service.Issue.Dto
{
    public class ExportIssueDto
    {
        [ExcelIgnore]
        [Comment("问题编号")]
        public long Id { get; set; }

        [ColumnExcelName("问题序号")]
        [Comment("问题序号")]
        public string SerialNumber { get; set; }

        [ColumnExcelName("问题简述")]
        [Comment("问题简述")]
        public string Title { get; set; }

        [ColumnExcelName("项目名称")]
        [Comment("项目名称")]
        public string ProjectName { get; set; }

        [ExcelIgnore]
        public long ProjectId { get; set; }

        [ColumnExcelName("产品名称")]
        [Comment("产品名称")]
        public string ProductName { get; set; }

        [ExcelIgnore]
        public long? ProductId { get; set; }

        [ColumnExcelName("问题模块")]
        [Comment("问题模块")]
        public string Module { get; set; }

        [ColumnExcelName("问题性质")]
        [Comment("问题性质")]
        public string Consequence { get; set; }

        [ColumnExcelName("问题分类")]
        [Comment("问题分类")]
        public string IssueClassification { get; set; }

        [ColumnExcelName("问题来源")]
        [Comment("问题来源")]
        public string Source { get; set; }

        [ColumnExcelName("问题状态")]
        [Comment("问题状态")]
        public string Status { get; set; }

        [NotToTableColumn]
        [ExcelIgnore]
        public EnumIssueStatus IssueStatus { get; set; }

        [ColumnExcelName("提出人")]
        [Comment("提出人")]
        public string Creator { get; set; }

        [NotToTableColumn]
        [ExcelIgnore]
        public long? CreatorId { get; set; }

        [ColumnExcelName("提出日期")]
        [Comment("提出日期")]
        public string CreateTime { get; set; }

        [ColumnExcelName("关闭日期")]
        [Comment("关闭日期")]
        public string CloseTime { get; set; }

        [ColumnExcelName("发现人")]
        [Comment("发现人")]
        public string Discover { get; set; }

        [ColumnExcelName("发现日期")]
        [Comment("发现日期")]
        public string DiscoverTime { get; set; }

        [ColumnExcelName("分发人")]
        [Comment("分发人")]
        public string Dispatcher { get; set; }

        [NotToTableColumn]
        [ExcelIgnore]
        public long? DispatcherId { get; set; }

        [ColumnExcelName("分发日期")]
        [Comment("分发日期")]
        public string DispatchTime { get; set; }

        [ColumnExcelName("预计完成日期")]
        [Comment("预计完成日期")]
        public string ForecastSolveTime { get; set; }

        [ColumnExcelName("被抄送人")]
        [Comment("被抄送人")]
        public string CC { get; set; }

        [ColumnExcelName("解决人")]
        [Comment("解决人")]
        public string Executor { get; set; }

        [NotToTableColumn]
        [ExcelIgnore]
        public long? ExecutorId { get; set; }

        [ColumnExcelName("解决日期")]
        [Comment("解决日期")]
        public string SolveTime { get; set; }

        [ColumnExcelName("验证人")]
        [Comment("验证人")]
        public string Verifier { get; set; }

        [ColumnExcelName("验证地点")]
        [Comment("验证地点")]
        public string VerifierPlace { get; set; }

        [ColumnExcelName("验证日期")]
        [Comment("验证日期")]
        public string ValidateTime { get; set; }

        [ColumnExcelName("问题详情")]
        [Comment("问题详情")]
        public string Description { get; set; }

        [ColumnExcelName("原因分析")]
        [Comment("原因分析")]
        public string Reason { get; set; }

        [ColumnExcelName("解决措施")]
        [Comment("解决措施")]
        public string Measures { get; set; }

        [ColumnExcelName("验证数量")]
        [Comment("验证数量")]
        public int? Count { get; set; }

        [ColumnExcelName("验证批次")]
        [Comment("验证批次")]
        public string Batch { get; set; }

        [ColumnExcelName("验证情况")]
        [Comment("验证情况")]
        public string Result { get; set; }

        [ColumnExcelName("验证状态")]
        [Comment("验证状态")]
        public string ValidationStatus { get; set; }

        [ColumnExcelName("解决版本")]
        [Comment("解决版本")]
        public string SolveVersion { get; set; }

        [ColumnExcelName("备注")]
        [Comment("备注")]
        public string Comment { get; set; }

        [ColumnExcelName("挂起情况")]
        [Comment("挂起情况")]
        public string HangupReason { get; set; }

        /// <summary>
        /// 用于新增和分发时保存动态生成的字段信息（动态生成对应控件时,字段结构可通过相应接口获得）
        /// module：模块 int
        /// issueId：问题编号 long
        /// fieldId：字段编号  long
        /// fieldCode：字段代码 string
        /// fieldName：字段代码的中文意思 string
        /// value：字段值 string
        /// valueType：字段类型 string
        /// [{"module": 0, "issueId":284932473958469,"fieldId":285613677277253,"fieldCode":"code", "fieldName":"中文代码", "value":"数据","fieldDataType":"string"}]
        /// </summary>
        [Comment("扩展属性")]
        public string ExtendAttribute { get; set; }

        public List<FileValueDto> ExtendAttributeList { get; set; }

        public ExportIssueDto(Core.Entity.Issue issue, IssueDetail detail)
        {
            this.Id = issue.Id;
            this.SerialNumber = issue.SerialNumber;
            this.Title = issue.Title;
            this.Module = issue.Module.GetEnumDescription();
            this.Consequence = issue.Consequence.GetEnumDescription();
            this.IssueClassification = issue.IssueClassification.GetEnumDescription();
            if (issue.Source != null)
            {
                this.Source = ((EnumIssueSource)issue.Source).GetEnumDescription();
            }
            this.Status = issue.Status.GetEnumDescription();
            this.CreateTime = issue.CreateTime.GetTimeString();
            this.DiscoverTime = issue.DiscoverTime.GetTimeString();
            this.DispatchTime = issue.DispatchTime.GetTimeString();
            this.ForecastSolveTime = issue.ForecastSolveTime.GetTimeString();
            this.SolveTime = issue.SolveTime.GetTimeString();
            this.ValidateTime = issue.ValidateTime.GetTimeString();
            this.VerifierPlace = issue.VerifierPlace;

            this.Creator = issue.CreatorId.GetNameByEmpId();
            this.Dispatcher = issue.Dispatcher.GetNameByEmpId();
            this.ProjectId = issue.ProjectId;
            this.ProjectName = issue.ProjectId.GetNameByProjectId();

            this.ProductId = issue.ProductId;
            if (this.ProductId != null)
            {
                this.ProductName = ((long)this.ProductId).GetNameByProductId();
            }
            this.Discover = issue.Discover.GetNameByEmpId();
            this.Executor = issue.Executor.GetNameByEmpId();
            this.Verifier = issue.Verifier == null ? this.Creator : issue.Verifier.GetNameByEmpId();

            this.ValidationStatus = issue.ValidationStatus == 0 ? "未验证" : (issue.ValidationStatus == 1 ? "验证不通过" : "验证通过");

            this.CloseTime = issue.CloseTime.GetTimeString();
            if (!string.IsNullOrEmpty(issue.CCs))
            {
                List<long> cc = JSON.Deserialize<List<long>>(issue.CCs);

                //this.CC = JSON.Serialize(cc.Select<long, string>(id => id.GetNameByEmpId()));
                this.CC = String.Join(",", cc.Select<long, string>(id => id.GetNameByEmpId()));
            }

            this.Description = detail.Description.FormatRichText();
            this.Batch = detail.Batch;
            this.Count = detail.Count;
            this.SolveVersion = detail.SolveVersion;
            this.Comment = detail.Comment;
            this.ExtendAttribute = detail.ExtendAttribute;
            if (!string.IsNullOrEmpty(detail.ExtendAttribute))
            {
                this.ExtendAttributeList = JSON.Deserialize<List<FileValueDto>>(detail.ExtendAttribute);
            }
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

    public class FileValueDto
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        public long IssueId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public EnumModule Module { get; set; }

        /// <summary>
        /// 字段编号
        /// 新增时忽略该字段
        /// </summary>
        public long FieldId { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [ColumnHead("ceshi2")]
        public string FieldName { get; set; }

        /// <summary>
        /// 字段代码
        /// </summary>
        public string FieldCode { get; set; }

        /// <summary>
        /// 字段数据类型
        /// </summary>
        public string FieldDataType { get; set; }

        /// <summary>
        /// 枚举类型下的显示值
        /// </summary>
        [ColumnValue("ceshi1")]
        public string DisplayValue
        {
            get
            {
                //枚举类型下转换显示
                if (FieldDataType == "enum")
                {
                    return Helper.Helper.GetEnumDisplayByCodeAndValue(this.FieldCode, this.Value);
                }
                else
                {
                    return this.Value;
                }
            }
        }
    }
}