using Furion.Extras.Admin.NET;
using QMS.Core.Enum;

namespace QMS.Application.Issues.Service.Issue.Dto.Query
{
    public class BaseQueryModel: PageInputBase
    {
        public long ProjectId { get; set; }
        public EnumModule? Module { get; set; }
        public EnumConsequence? Consequence { get; set; }
        public EnumIssueStatus? Status { get; set; }
        public string Title { get; set; }


        public long? Creator { get; set; } // 提出人
        public long? Dispatcher { get; set; } // 转发人
        public long? Executor { get; set; } // 解决人

        public DateTime? CreateTimeFrom { get; set; } // 提出时间起点
        public DateTime? CreateTimeTo { get; set; } // 提出时间终点

        public DateTime? DispatchTimeFrom { get; set; } // 转发时间起点
        public DateTime? DispatchTimeTo { get; set; } // 转发时间终点

        public DateTime? SolveTimeFrom { get; set; } // 解决时间起点
        public DateTime? SolveTimeTo { get; set; } // 解决时间终点


        public EnumQueryCondition QueryCondition { get; set; }
    }

    public enum EnumQueryCondition
    {
        General,
        Creator,
        Dispatcher,
        Executor,
        Solved,
        Unsolve,
        Closed,
        Hangup,
        CC
    }
}
