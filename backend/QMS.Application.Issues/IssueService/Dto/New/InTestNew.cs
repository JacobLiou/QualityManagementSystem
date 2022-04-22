using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.IssueService.Dto.New
{
    public class InTestNew : BaseId
    {
        public int TestClassification { get; set; }
        public string ProductType { get; set; }
        public int DeliverSampleCount { get; set; }
        public string DeliverTestDate { get; set; }
        public long TestDirectorId { get; set; }
        public string OverTestDate { get; set; }
        public string DSPM { get; set; }
        public string DSPS { get; set; }
        public string ARM { get; set; }

        public string PCB { get; set; }
        /// <summary>
        /// 3,6,5
        /// </summary>
        public string PrototypeDescriptionId { get; set; }
        public long DevelopmentDirectorId { get; set; }
    }
}
