using Furion.Extras.Admin.NET;
using QMS.Core.Enum;

namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class BaseQueryModel: PageInputBase
    {
        public long ProjectId { get; set; }
        public EnumModule? Module { get; set; }
        public EnumConsequence? Consequence { get; set; }
        public EnumIssueStatus? Status { get; set; }
        public string KeyWord { get; set; }

        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
