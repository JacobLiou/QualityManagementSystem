using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public class InReDispatch : ReDispatchCommon
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        public override bool SetIssueDetail(IssueDetail issueDetail)
        {
            bool changed = false;

            if (issueDetail.Comment != this.Comment)
            {
                issueDetail.Comment = this.Comment;

                changed = true;
            }

            return changed;
        }
    }
}
