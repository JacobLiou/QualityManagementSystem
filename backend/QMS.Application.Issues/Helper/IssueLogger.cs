using Furion.DatabaseAccessor;
using QMS.Core;
using QMS.Core.Entity;

namespace QMS.Application.Issues.Helper
{
    public static class IssueLogger
    {
        public static async Task Log(
            IRepository<IssueOperation, IssuesDbContextLocator> issueOperateRep, 
            long id, 
            Core.Enum.EnumIssueOperationType enumIssueOperationType, 
            string content
        )
        {
            await issueOperateRep.InsertAsync(new IssueOperation()
            {
                IssueId = id,
                OperationTypeId = enumIssueOperationType,
                OperationTime = DateTime.Now,
                OperatorName = Helper.GetCurrentUser().GetNameByEmpId(),
                Content = content
            }, true);
        }
    }
}
