using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.IssueService.Dto.Detail
{
    public class OutputDetailIssue : BaseId
    {
        public string SolveVersion { get; set; }
        public int Count { get; set; }
        public string Batch { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string Measures { get; set; }
        public string Result { get; set; }
        /// <summary>
        /// Json Array
        /// </summary>
        public string Attachments { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string ExtendAttribute { get; set; }
    }
}
