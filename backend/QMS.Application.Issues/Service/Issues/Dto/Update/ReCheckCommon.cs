using Furion.Extras.Admin.NET;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public abstract class ReCheckCommon : IInput
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
        /// 复核结果，1：有效 0：无效
        /// </summary>
        public YesOrNot PassResult { get; set; }


        public bool SetIssue(Core.Entity.Issue issue)
        {
            Helper.Helper.Assert(issue.Status == Core.Enum.EnumIssueStatus.Solved, Constants.ERROR_MSG_CHECK_RECHECK);

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
