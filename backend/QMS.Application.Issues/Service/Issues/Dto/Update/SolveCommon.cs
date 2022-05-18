namespace QMS.Application.Issues.Service.Issue.Dto.Update
{
    public class SolveCommon
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
        /// 解决人
        /// </summary>
        //public long? Executor { get; set; }

        /// <summary>
        /// 解决日期
        /// </summary>
        public DateTime SolveTime { get; set; }
    }
}
