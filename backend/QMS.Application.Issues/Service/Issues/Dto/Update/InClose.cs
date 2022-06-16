using QMS.Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issues.Dto.Update
{
    /// <summary>
    /// 关闭情况
    /// </summary>
    public class InClose : CloseCommon
    {
        /// <summary>
        /// 关闭原因
        /// </summary>
        [Required]
        public string CloseReason { set; get; }

        public override bool SetIssueDetail(IssueDetail issueDetail)
        {
            Helper.Helper.Assert(!string.IsNullOrEmpty(CloseReason), "关闭原因不能为空");
            bool changed = false;
            if (issueDetail.CloseReason != this.CloseReason)
            {
                issueDetail.CloseReason = this.CloseReason;

                changed = true;
            }
            return changed;
        }
    }
}