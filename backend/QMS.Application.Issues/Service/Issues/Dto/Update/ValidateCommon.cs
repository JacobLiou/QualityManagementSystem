using Furion.Extras.Admin.NET;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
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
        //public long? Verifier { get; set; }

        /// <summary>
        /// 验证地点
        /// </summary>
        public string VerifierPlace { get; set; }

        /// <summary>
        /// 验证日期
        /// </summary>
        //public DateTime? ValidateTime { get; set; }

        public YesOrNot PassResult { get; set; }

        public bool SetIssue(Core.Entity.Issue issue)
        {
            Helper.Helper.Assert(issue.Status == Core.Enum.EnumIssueStatus.Solved && issue.Verifier != null, Constants.ERROR_MSG_CHECK_VALIDATE);

            bool changed = false;

            if (issue.Title != this.Title)
            {
                issue.Title = this.Title;

                changed = true;
            }

            if (issue.VerifierPlace != this.VerifierPlace)
            {
                issue.VerifierPlace = this.VerifierPlace;

                changed = true;
            }

            return changed;
        }

        public abstract bool SetIssueDetail(IssueDetail issueDetail);
    }
}
