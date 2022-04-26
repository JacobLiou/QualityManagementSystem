namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public class HangupDetail
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 挂起情况
        /// </summary>
        public string HangupReason { get; set; }
    }
}
