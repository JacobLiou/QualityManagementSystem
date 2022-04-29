using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public abstract class ReDispatchCommon:IInput
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

        public bool SetIssue(Core.Entity.SsuIssue issue)
        {
            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }

            return changed;
        }

        public abstract bool SetIssueDetail(SsuIssueDetail issueDetail);
    }
}
