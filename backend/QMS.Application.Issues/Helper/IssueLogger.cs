using Furion.DatabaseAccessor;
using QMS.Core;
using QMS.Core.Entity;

namespace QMS.Application.Issues.Helper
{
    public static class IssueLogger
    {
        public static async Task Log(
            IRepository<SsuIssueOperation, IssuesDbContextLocator> ssuIssueOperateRep, 
            long id, 
            Core.Enum.EnumIssueOperationType enumIssueOperationType, 
            string content
        )
        {
            await ssuIssueOperateRep.InsertAsync(new SsuIssueOperation()
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
