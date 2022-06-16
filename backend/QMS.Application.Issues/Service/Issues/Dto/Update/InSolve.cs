using Furion.Extras.Admin.NET;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public class InSolve : SolveCommon, IInput
    {
        /// <summary>
        /// 原因分析
        /// </summary>
        [Required]
        public string Reason { get; set; }

        /// <summary>
        /// 解决措施
        /// </summary>
        [Required]
        public string Measures { get; set; }

        /// <summary>
        /// 解决版本
        /// </summary>
        public string SolveVersion { get; set; }

        public bool SetIssue(Core.Entity.Issue issue)
        {
            Helper.Helper.Assert(issue.Status > Core.Enum.EnumIssueStatus.Created && issue.CurrentAssignment != null, Constants.ERROR_MSG_CHECK_SOLVE);

            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }

            //设置解决日期
            issue.SolveTime = this.SolveTime != default(DateTime) ? this.SolveTime : DateTime.Now;
            issue.Executor = CurrentUserInfo.UserId;
            issue.Status = EnumIssueStatus.Solved;
            issue.CurrentAssignment = issue.Dispatcher;

            return changed;
        }

        public bool SetIssueDetail(IssueDetail issueDetail)
        {
            bool changed = false;

            Helper.Helper.Assert(this.Reason != null && this.Measures != null, "原因分析和解决措施不允许为空!");

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