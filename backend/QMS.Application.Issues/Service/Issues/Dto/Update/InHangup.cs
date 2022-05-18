using QMS.Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public class InHangup : HangupCommon
    {
        /// <summary>
        /// 挂起情况
        /// </summary>
        [Required]
        public string HangupReason { get; set; }

        public override bool SetIssueDetail(IssueDetail issueDetail)
        {
            bool changed = false;

            Helper.Helper.Assert(this.HangupReason != null, "挂起情况不允许为空");

            if (issueDetail.HangupReason != this.HangupReason)
            {
                issueDetail.HangupReason = this.HangupReason;

                changed = true;
            }

            return changed;
        }
    }
}
