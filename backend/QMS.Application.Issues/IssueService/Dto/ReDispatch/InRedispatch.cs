using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.IssueService.Dto.ReDispatch
{
    public class InRedispatch : BaseId
    {
        public string Comment { get; set; }
        public long Exector { get; set; }
    }
}
