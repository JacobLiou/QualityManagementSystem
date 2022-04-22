namespace QMS.Application.Issues.IssueService.Dto.Dispatch
{
    public class InOutCommonTestDispatch
    {
        //public long IssueId { get; set; }
        public int IssueClassification { get; set; }
        public int Consequence { get; set; }
        public long DirectorId { get; set; }
        public string ForecastSolveTime { get; set; }
        public long CC { get; set; }
    }
}
