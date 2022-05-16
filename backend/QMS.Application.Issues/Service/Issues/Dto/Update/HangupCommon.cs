using QMS.Application.Issues.Helper;
using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public abstract class HangupCommon : IInput
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 问题简述
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 挂起人
        /// </summary>
        //public long? HangupId { get; set; }

        public bool SetIssue(Core.Entity.Issue issue)
        {
            Helper.Helper.Assert(issue.Status != Core.Enum.EnumIssueStatus.Closed, Constants.ERROR_MSG_CHECK_HANGUP);

            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }
            return changed;
        }

        public abstract bool SetIssueDetail(IssueDetail issueDetail);
    }
}
