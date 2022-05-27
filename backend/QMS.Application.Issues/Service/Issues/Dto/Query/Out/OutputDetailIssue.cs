using Furion.Extras.Admin.NET.Service;
using QMS.Application.Issues.Helper;

namespace QMS.Application.Issues.Service.Issue.Dto.Query
{
    public class OutputDetailIssue : BaseId
    {
        /// <summary>
        /// 解决版本
        /// </summary>
        public string SolveVersion { get; set; }
        /// <summary>
        /// 验证情况
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 验证数量
        /// </summary>
        public int? Count { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 问题详情
        /// </summary>
        public string Description { get; set; }

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
        public string ExtendAttribute { get; set; }

        /// <summary>
        /// 解决措施
        /// </summary>
        public string Measures { get; set; }
        /// <summary>
        /// 挂起原因
        /// </summary>
        public string HangupReason { get; set; }
        /// <summary>
        /// 原因分析
        /// </summary>
        public string Reason { get; set; }

        public void SetCommon(QMS.Core.Entity.Issue issue)
        {
            this.Title = issue.Title;
            this.ProductId = issue.ProductId;
            this.ProductName = issue.ProductId?.GetNameByProductId();
            this.ProjectId = issue.ProjectId;
            this.ProjectName = issue.ProjectId.GetNameByProjectId();
            this.Module = issue.Module;
            this.IssueClassification = issue.IssueClassification;
            if (issue.Dispatcher != null)
            {
                this.DispatcherName = issue.Dispatcher.GetNameByEmpId();
            }
            this.Consequence = issue.Consequence;
            this.Source = issue.Source;
            this.DiscoverName = issue.Discover.GetNameByEmpId();
            this.DiscoverTime = issue.DiscoverTime;
            this.Status = issue.Status;
            this.CreatorName = issue.CreatorId.GetNameByEmpId();
            this.CreateTime = issue.CreateTime;
            this.CCList = issue.CCs;
        }

        // 公共问题属性
        /// <summary>
        /// 问题简述
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 项目名
        /// </summary>
        public virtual string ProjectName { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 问题模块
        /// </summary>
        public virtual Core.Enum.EnumModule Module { get; set; }

        /// <summary>
        /// 问题分类
        /// </summary>
        public virtual Core.Enum.EnumIssueClassification IssueClassification { get; set; }

        /// <summary>
        /// 分发人
        /// </summary>
        public virtual string DispatcherName { get; set; }

        /// <summary>
        /// 问题性质
        /// </summary>
        public virtual Core.Enum.EnumConsequence Consequence { get; set; }

        /// <summary>
        /// 问题来源
        /// </summary>
        public virtual Core.Enum.EnumIssueSource? Source { get; set; }

        /// <summary>
        /// 发现人
        /// </summary>
        public virtual string DiscoverName { get; set; }

        /// <summary>
        /// 发现日期
        /// </summary>
        public virtual DateTime? DiscoverTime { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public virtual Core.Enum.EnumIssueStatus? Status { get; set; }

        /// <summary>
        /// 提出人
        /// </summary>
        public virtual string CreatorName { get; set; }

        /// <summary>
        /// 提出日期
        /// </summary>
        public virtual DateTime? CreateTime { get; set; }

        /// <summary>
        /// 被抄送人，可选多个
        /// </summary>
        public virtual string CCList { get; set; }
    }
}
