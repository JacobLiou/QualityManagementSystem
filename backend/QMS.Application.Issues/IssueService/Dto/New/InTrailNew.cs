using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.IssueService.Dto.New
{
    public class InOutTrailDispatch : BaseId
    {
        public int Veneer { get; set; }
        public int Process { get; set; }
        public string ProductCode { get; set; }
        public string MachineCode { get; set; }
        public string WorkOrderId { get; set; }
        public string MaterialNum { get; set; }
        public string TrailProduceDate { get; set; }
        public int TrailProduceCount { get; set; }
        public string TrailProducePlace { get; set; }
        public string TrailBatch { get; set; }
        public double OKRatio { get; set; }
    }
}
