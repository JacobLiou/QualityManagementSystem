using Furion.Extras.Admin.NET;
using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public abstract class ValidateCommon : IInput
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
        /// 验证人
        /// </summary>
        public long Verifier { get; set; }

        /// <summary>
        /// 验证地点
        /// </summary>
        public string VerifierPlace { get; set; }

        /// <summary>
        /// 验证日期
        /// </summary>
        public DateTime? ValidateTime { get; set; }

        public YesOrNot PassResult { get; set; }

        public bool SetIssue(Core.Entity.SsuIssue issue)
        {
            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }

            if (issue.Verifier != this.Verifier)
            {
                issue.Verifier = this.Verifier;

                changed = true;
            }

            if (issue.ValidateTime != this.ValidateTime)
            {
                issue.ValidateTime = this.ValidateTime;

                changed = true;
            }

            return changed;
        }

        public abstract bool SetIssueDetail(SsuIssueDetail issueDetail);
    }
}
