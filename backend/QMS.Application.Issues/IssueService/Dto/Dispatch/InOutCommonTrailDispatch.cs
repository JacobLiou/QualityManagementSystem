namespace QMS.Application.Issues.IssueService.Dto.Dispatch
{
    public class InOutCommonTrailDispatch : InOutCommonTestDispatch
    {
        /// <summary>
        /// 不超过10
        /// </summary>
        public decimal ImpactScore { get; set; }
    }
}
