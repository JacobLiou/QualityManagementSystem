using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.IssueService.Dto.Validate
{
    public class InHangup : BaseId
    {
        public int Count { get; set; }
        public string Batch { get; set; }
        public string Attachments { get; set; }
    }
}
