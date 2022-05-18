using Furion.Extras.Admin.NET;
using QMS.Core.Enum;
using System.ComponentModel;

namespace QMS.Application.Issues.Service.Issue.Dto.Query
{
    public class BaseQueryModel: PageInputBase
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public long? ProjectId { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public EnumModule? Module { get; set; }
        /// <summary>
        /// 问题性质
        /// </summary>
        public EnumConsequence? Consequence { get; set; }
        /// <summary>
        /// 问题状态
        /// </summary>
        public EnumIssueStatus? Status { get; set; }
        /// <summary>
        /// 问题简述
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 提出人
        /// </summary>
        public long? Creator { get; set; } // 提出人
        /// <summary>
        /// 转发人
        /// </summary>
        public long? Dispatcher { get; set; } // 转发人
        /// <summary>
        /// 解决人
        /// </summary>
        public long? Executor { get; set; } // 解决人

        /// <summary>
        /// 查询条件：提出时间起点
        /// </summary>
        public DateTime? CreateTimeFrom { get; set; } // 提出时间起点
        /// <summary>
        /// 查询条件：提出时间终点
        /// </summary>
        public DateTime? CreateTimeTo { get; set; } // 提出时间终点
        /// <summary>
        /// 转发时间起点
        /// </summary>
        public DateTime? DispatchTimeFrom { get; set; } // 转发时间起点
        /// <summary>
        /// 转发时间终点
        /// </summary>
        public DateTime? DispatchTimeTo { get; set; } // 转发时间终点
        /// <summary>
        /// 解决时间起点
        /// </summary>
        public DateTime? SolveTimeFrom { get; set; } // 解决时间起点
        /// <summary>
        /// 解决时间终点
        /// </summary>
        public DateTime? SolveTimeTo { get; set; } // 解决时间终点

        /// <summary>
        /// 查询条件：我创建或我解决
        /// </summary>
        public EnumQueryCondition QueryCondition { get; set; }

        /// <summary>
        /// 问题分类
        /// </summary>
        public EnumIssueClassification? IssueClassification { get; set; }

        /// <summary>
        /// 试产工序
        /// </summary>
        public EnumTrialProductionProcess? TrialProductionProcess { get; set; }

        /// <summary>
        /// 测试类别
        /// </summary>
        public EnumTestClassification? TestClassification { get; set; }
    }

    public enum EnumQueryCondition
    {
        [Description("所有")]
        General,
        [Description("由我创建")]
        Creator,
        /// <summary>
        /// 当前流程在我这里：我可以是分发者、管理者、验证者
        /// </summary>
        [Description("指派给我")]
        AssignToMe,
        [Description("由我解决")]
        Executor,
        [Description("已处理")]
        Solved,
        [Description("未解决")]
        Unsolve,
        [Description("未关闭")]
        Closed,
        [Description("已挂起")]
        Hangup,
        //[Description("抄送给我")]
        //CC
    }
}
