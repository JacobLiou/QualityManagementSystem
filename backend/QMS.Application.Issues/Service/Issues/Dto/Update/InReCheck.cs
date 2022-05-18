using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    /// <summary>
    /// 用于分发人检查执行人执行措施是否有效
    /// </summary>
    public class InReCheck : ReCheckCommon
    {
        /// <summary>
        /// 复核情况
        /// </summary>
        public string ReCheckResult { get; set; }

        public override bool SetIssueDetail(IssueDetail issueDetail)
        {
            bool changed = false;

            if (issueDetail.ReCheckResult != this.ReCheckResult)
            {
                issueDetail.ReCheckResult = this.ReCheckResult;

                changed = true;
            }

            return changed;
        }
    }
}
