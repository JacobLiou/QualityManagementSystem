using Furion.Extras.Admin.NET;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public abstract class ReDispatchCommon : IInput
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
        /// 当前指派人
        /// </summary>
        [Required]
        public long CurrentAssignment { get; set; }

        //[Comment("预计完成日期")]
        //public DateTime? ForecastSolveTime { get; set; }

        public bool SetIssue(Core.Entity.Issue issue)
        {
            Helper.Helper.Assert(issue.Status != Core.Enum.EnumIssueStatus.Created || issue.Status != Core.Enum.EnumIssueStatus.Dispatched || issue.Status != Core.Enum.EnumIssueStatus.UnSolve, Constants.ERROR_MSG_CHECK_REDISPATCH);

            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }
            //转交操作，当前问题的各种状态值应保持不变，只修改当前指派人
            if (issue.CurrentAssignment != this.CurrentAssignment)
            {
                issue.CurrentAssignment = this.CurrentAssignment;
            }
            //issue.Executor = this.Executor;
            //issue.DispatchTime = DateTime.Now;
            //issue.Dispatcher = CurrentUserInfo.UserId;
            //issue.Status = EnumIssueStatus.Dispatched;

            return changed;
        }

        public abstract bool SetIssueDetail(IssueDetail issueDetail);
    }
}