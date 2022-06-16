using QMS.Application.Issues.Service.Issue.Dto.Update;
using QMS.Core.Entity;
using QMS.Core.Enum;

namespace QMS.Application.Issues.Service.Issues.Dto.Update
{
    /// <summary>
    /// 关闭
    /// </summary>
    public abstract class CloseCommon : IInput
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 问题简述
        /// </summary>
        public string Title { get; set; }

        public bool SetIssue(Core.Entity.Issue issue)
        {
            bool changed = false;
            //关闭操作
            issue.CloseTime = DateTime.Now;
            issue.Status = EnumIssueStatus.Closed;

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