using QMS.Core.Enum;

namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class QueryListInUnSolve : BaseQueryModel
    {
        public int Status { get; set; } = (int)EnumIssueStatus.UnSolve;
    }
}
