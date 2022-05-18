using Furion.JsonSerialization;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.Service.Issue.Dto.Update;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issues.Dto.Update
{
    public class InDispatch : IInput
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        [Required]
        public long Id { get; set; }
        /// <summary>
        /// 问题简述
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// 预计完成时间
        /// </summary>
        [Required]
        public DateTime ForecastSolveTime { get; set; }
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
        /// 执行人
        /// </summary>
        [Required]
        public long Executor { get; set; }
        /// <summary>
        /// 抄送给
        /// </summary>
        public List<long> CCList { get; set; } = new List<long>();

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

        public bool SetIssue(Core.Entity.Issue issue)
        {
            Helper.Helper.Assert(issue.Status == EnumIssueStatus.Created || issue.Status == EnumIssueStatus.HasHangUp, Constants.ERROR_MSG_CHECK_DISPATCH);

            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }

            issue.IssueClassification = this.IssueClassification;
            issue.Consequence = this.Consequence;
            issue.Executor = this.Executor;
            if (this.CCList != null)
            {
                issue.CCs = JSON.Serialize(this.CCList);
            }
            issue.ForecastSolveTime = this.ForecastSolveTime;

            return changed;
        }

        public bool SetIssueDetail(IssueDetail issueDetail)
        {

            return true;
        }
    }
}
