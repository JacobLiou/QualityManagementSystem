using Furion.Extras.Admin.NET.Entity.Common.Enum;

namespace QMS.Application.Issues.Service.Issue.Attachment
{
    public class AttachmentModel
    {
        //public long IssueId { get; set; }
        /// <summary>
        /// 附件编号
        /// </summary>
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public EnumAttachmentType AttachmentType { get; set; }
    }
}
