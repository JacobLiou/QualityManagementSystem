using QMS.Core.Entity;

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
        /// 解决人
        /// </summary>
        public long Executor { get; set; }

        //[Comment("预计完成日期")]
        //public DateTime? ForecastSolveTime { get; set; }

        public bool SetIssue(Core.Entity.Issue issue)
        {
            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }

            if (issue.Executor != this.Executor)
            {
                issue.Executor = this.Executor;

                changed = true;
            }

            //if (issue.ForecastSolveTime != this.ForecastSolveTime)
            //{
            //    issue.ForecastSolveTime = this.ForecastSolveTime;

            //    changed = true;
            //}

            return changed;
        }

        public abstract bool SetIssueDetail(IssueDetail issueDetail);
    }
}
