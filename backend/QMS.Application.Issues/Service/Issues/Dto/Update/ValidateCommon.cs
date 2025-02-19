﻿using Furion.Extras.Admin.NET;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;

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

        /// <summary>
        /// 验证结果，1：通过 0：不通过，即未解决
        /// </summary>
        public YesOrNot PassResult { get; set; }

        public bool SetIssue(Core.Entity.Issue issue)
        {
            Helper.Helper.Assert(issue.Status == Core.Enum.EnumIssueStatus.HasRechecked, Constants.ERROR_MSG_CHECK_VALIDATE);

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
            //验证结果
            bool pass = this.PassResult == YesOrNot.Y;
            issue.Verifier = CurrentUserInfo.UserId;
            issue.ValidateTime = DateTime.Now;
            issue.Status = pass ? EnumIssueStatus.Closed : EnumIssueStatus.UnSolve;
            issue.ValidationStatus = pass ? 2 : 1;
            issue.CurrentAssignment = pass ? issue.CreatorId : issue.Dispatcher;
            if (pass)
            {
                issue.CloseTime = DateTime.Now;
            }

            return changed;
        }

        public abstract bool SetIssueDetail(IssueDetail issueDetail);
    }
}