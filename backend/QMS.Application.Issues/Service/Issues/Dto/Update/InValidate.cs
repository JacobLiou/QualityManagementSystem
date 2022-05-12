using QMS.Core.Entity;

namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public class InValidate : ValidateCommon
    {
        /// <summary>
        /// 验证数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 验证批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 验证情况
        /// </summary>
        public string Result { get; set; }

        public override bool SetIssueDetail(IssueDetail issueDetail)
        {
            bool changed = false;

            if (issueDetail.Count != this.Count)
            {
                issueDetail.Count = this.Count;

                changed = true;
            }

            if (issueDetail.Batch != this.Batch)
            {
                issueDetail.Batch = this.Batch;

                changed = true;
            }

            if (issueDetail.Result != this.Result)
            {
                issueDetail.Result = this.Result;

                changed = true;
            }

            return changed;
        }
    }
}
