using Furion.Extras.Admin.NET.Entity.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issue.Attachment
{
    public class AttachmentModel
    {
        //public long IssueId { get; set; }
        /// <summary>
        /// 附件编号
        /// </summary>
        [Required]
        public long AttachmentId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        [Required]
        public string FileName { get; set; }
        [Required]
        public EnumAttachmentType AttachmentType { get; set; }
    }
}
