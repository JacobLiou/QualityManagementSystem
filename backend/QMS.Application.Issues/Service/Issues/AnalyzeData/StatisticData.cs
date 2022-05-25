namespace QMS.Application.Issues.Service.Issues.AnalyzeData
{
    public class StatisticData
    {
        /// <summary>
        /// 致命问题数量
        /// </summary>
        public int DeadlyConsequenceCount { get; set; }

        /// <summary>
        /// 严重问题数量
        /// </summary>
        public int SeriousConsequenceCount { get; set; }

        /// <summary>
        /// 指派给我的问题数量
        /// </summary>
        public int AssignToMeCount { get; set; }

        /// <summary>
        /// 已处理的问题数量
        /// </summary>
        public int SolvedCount { get; set; }

        public static StatisticData DefaultEmpty => new StatisticData();
    }
}
