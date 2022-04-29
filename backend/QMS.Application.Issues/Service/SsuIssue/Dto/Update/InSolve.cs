using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public class InSolve : SolveCommon, IInput
    {
        /// <summary>
        /// 原因分析
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 解决措施
        /// </summary>
        public string Measures { get; set; }

        /// <summary>
        /// 解决版本
        /// </summary>
        public string SolveVersion { get; set; }

        public bool SetIssue(Core.Entity.SsuIssue issue)
        {
            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }

            return changed;
        }

        public bool SetIssueDetail(SsuIssueDetail issueDetail)
        {
            bool changed = false;

            if (issueDetail.Reason != this.Reason)
            {
                issueDetail.Reason = this.Reason;

                changed = true;
            }

            if (issueDetail.Measures != this.Measures)
            {
                issueDetail.Measures = this.Measures;

                changed = true;
            }

            if (issueDetail.SolveVersion != this.SolveVersion)
            {
                issueDetail.SolveVersion = this.SolveVersion;

                changed = true;
            }

            return changed;
        }
    }
}
