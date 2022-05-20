using QMS.Core.Enum;

namespace QMS.Application.Issues.Service.Issues.AnalyzeData
{
    internal class IssuePropertyDto
    {
        public EnumConsequence Consequence { get; set; }
        public long CreatorId { get; set; }
        public long? Dispatcher { get; set; }
        public long? Executor { get; set; }
        public long? CurrentAssignment { get; set; }
        public EnumIssueStatus Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
