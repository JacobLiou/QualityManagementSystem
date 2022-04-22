using Furion.Extras.Admin.NET.Service;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;

namespace QMS.Application.Issues.IssueService.Dto.OperationRecord
{
    public class QueryOperationListByIssueId : BaseId
    {
        public QueryOperationListByIssueId(SsuIssueOperation model)
        {
            base.Id = model.IssueId;
            this.OperationType = model.OperationTypeId.GetEnumDescription<EnumIssueOperationType>();
            this.Content = model.Content;
            this.OperationTime = model.OperationTime.GetTimeString();
        }

        public string OperationType { get; set; }
        public string Content { get; set; }
        public string OperationTime { get; set; }
    }
}
