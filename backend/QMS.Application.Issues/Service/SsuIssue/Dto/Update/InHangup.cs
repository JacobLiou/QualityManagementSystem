using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public class InHangup : HangupCommon
    {
        /// <summary>
        /// 挂起情况
        /// </summary>
        public string HangupReason { get; set; }

        public override bool SetIssueDetail(SsuIssueDetail issueDetail)
        {
            bool changed = false;

            if (issueDetail.HangupReason != this.HangupReason)
            {
                issueDetail.HangupReason = this.HangupReason;

                changed = true;
            }

            return changed;
        }
    }
}
