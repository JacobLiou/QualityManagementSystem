namespace QMS.Application.Issues.Service.SsuIssue.Dto.Add
{
    public class InIssue: AddToCommonIssue
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
