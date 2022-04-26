using Furion.Extras.Admin.NET;
using QMS.Core.Enum;

namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class BaseQueryModel: PageInputBase
    {
        public long ProjectId { get; set; }
        public EnumModule? Module { get; set; }
        public EnumConsequence? Consequence { get; set; }
        public virtual EnumIssueStatus? Status { get; set; }
        public string KeyWord { get; set; }

        public EnumQueryCondition QueryCondition { get; set; }
    }

    public enum EnumQueryCondition
    {
        General,
        Creator,
        Dispatcher,
        Executor,
        Solved,
        Unsolve
    }
}
