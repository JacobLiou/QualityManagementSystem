namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public interface IInput
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="issue"></param>
        /// <returns>是否有更新</returns>
        bool SetIssue(QMS.Core.Entity.SsuIssue issue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueDetail"></param>
        /// <returns>是否有更新</returns>
        bool SetIssueDetail(QMS.Core.Entity.SsuIssueDetail issueDetail);
    }
}
