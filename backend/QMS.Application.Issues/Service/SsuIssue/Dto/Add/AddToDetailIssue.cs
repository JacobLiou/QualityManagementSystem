using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.Service.SsuIssue.Dto.Add
{
    internal class AddToDetailIssue : BaseId
    {
        /// <summary>
        /// 详情
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Json ["url1","url2"]
        /// </summary>
        public string Attachments { get; set; }
    }
}
