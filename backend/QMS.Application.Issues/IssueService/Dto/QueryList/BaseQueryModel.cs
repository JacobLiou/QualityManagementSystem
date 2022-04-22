namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class BaseQueryModel
    {
        public long ProjectId { get; set; }
        public int Module { get; set; }
        public int Consequence { get; set; }
        public int Status { get; set; }
        public string KeyWord { get; set; }

        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
