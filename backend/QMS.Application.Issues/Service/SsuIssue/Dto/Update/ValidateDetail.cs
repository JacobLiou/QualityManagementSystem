namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public class ValidateDetail
    {
        /// <summary>
        /// 问题编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 验证数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 验证批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 验证情况
        /// </summary>
        public string Result { get; set; }
    }
}
