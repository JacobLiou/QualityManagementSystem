using Furion.Extras.Admin.NET.Entity.Common.Enum;

namespace QMS.Application.Issues.Service.SsuIssue.Attachment
{
    internal class AttachmentModel
    {
        //public long IssueId { get; set; }
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public EnumAttachmentType AttachmentType { get; set; }
    }
}
