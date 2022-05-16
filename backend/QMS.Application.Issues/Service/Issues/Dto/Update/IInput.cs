namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public interface IInput
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="issue"></param>
        /// <returns>是否有更新</returns>
        bool SetIssue(Core.Entity.Issue issue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueDetail"></param>
        /// <returns>是否有更新</returns>
        bool SetIssueDetail(Core.Entity.IssueDetail issueDetail);
    }
}
