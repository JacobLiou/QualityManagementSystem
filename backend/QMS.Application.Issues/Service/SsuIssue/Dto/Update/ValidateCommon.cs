namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public class ValidateCommon
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
        /// 验证人
        /// </summary>
        public long Verifier { get; set; }

        /// <summary>
        /// 验证地点
        /// </summary>
        public string VerifierPlace { get; set; }

        /// <summary>
        /// 验证日期
        /// </summary>
        public DateTime ValidateTime { get; set; }
    }
}
