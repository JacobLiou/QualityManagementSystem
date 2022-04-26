using QMS.Core.Enum;

namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class QueryListInSolved : BaseQueryModel
    {
        public override EnumIssueStatus? Status { get; set; } = EnumIssueStatus.Solved;
    }
}
