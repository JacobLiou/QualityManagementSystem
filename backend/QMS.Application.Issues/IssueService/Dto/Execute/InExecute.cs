using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.IssueService.Dto.Execute
{
    public class InValidate : BaseId
    {
        /// <summary>
        /// 公共问题表
        /// </summary>
        public string SolveTime { get; set; }
        public string SolveVersion { get; set; }
        public string Reason { get; set; }
        public string Measures { get; set; }
        public string Attachments { get; set; }
    }
}
