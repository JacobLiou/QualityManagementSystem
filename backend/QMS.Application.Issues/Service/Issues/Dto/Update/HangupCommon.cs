using Furion;
using Furion.Extras.Admin.NET;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;

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

            Helper.Helper.Assert(Helper.Helper.GetCurrentUser() == issue.CurrentAssignment, "当前用户不是分发用户，无法执行挂起操作");

            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }
            issue.HangupId = CurrentUserInfo.UserId;
            issue.Status = EnumIssueStatus.HasHangUp;

            return changed;
        }

        public abstract bool SetIssueDetail(IssueDetail issueDetail);
    }
}