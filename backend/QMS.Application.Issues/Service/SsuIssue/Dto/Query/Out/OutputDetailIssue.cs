using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class OutputDetailIssue : BaseId
    {
        public string SolveVersion { get; set; }
        public string Result { get; set; }
        public string Batch { get; set; }
        public int? Count { get; set; }

        public string Comment { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// List<FieldValue>.ToJson()
        /// </summary>
        public string ExtendAttribute { get; set; }

        public string Measures { get; set; }
        public string HangupReason { get; set; }
        public string Reason { get; set; }
    }
}
