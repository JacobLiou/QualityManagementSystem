namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public class HangupCommon
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 问题简述
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 挂起人
        /// </summary>
        public long hangupId { get; set; }
    }
}
