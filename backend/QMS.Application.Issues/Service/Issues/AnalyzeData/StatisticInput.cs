namespace QMS.Application.Issues.Service.Issues.AnalyzeData
{
    public class StatisticInput
    {
        /// <summary>
        /// 查询(创建)问题起始时间
        /// </summary>
        public DateTime From { get; set; }
        /// <summary>
        /// 查询(创建)问题结束时间
        /// </summary>
        public DateTime To { get; set; }
    }
}
