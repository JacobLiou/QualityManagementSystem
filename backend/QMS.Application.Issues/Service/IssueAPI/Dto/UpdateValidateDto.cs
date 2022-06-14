using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues.Service.IssueAPI
{
    /// <summary>
    /// 更新验证结果
    /// </summary>
    public class UpdateValidateDto
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        public string IssueId { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool Validate { get; set; }
        /// <summary>
        /// 验证结果
        /// </summary>
        public string ValidateResult { get; set; }
        /// <summary>
        /// 验证人名称
        /// </summary>
        public string VerifierName { get; set; }
        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime ValidateTime { get; set; }

        /// <summary>
        /// 附件ID
        /// </summary>
        public List<long> Attachments { get; set; }
    }
}
