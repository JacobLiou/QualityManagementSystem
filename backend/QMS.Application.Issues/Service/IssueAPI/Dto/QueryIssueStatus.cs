using Furion.Extras.Admin.NET.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues.Service.IssueAPI
{
    /// <summary>
    /// 查询问题状态
    /// </summary>
    [Serializable]
    public class IssueStatusDto
    {
        /// <summary>
        /// 问题状态
        /// </summary>
        public int IssueStatus { get; set; }

        /// <summary>
        /// 操作记录
        /// </summary>
        public IList<IssueOperationDto> Operations { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 解决措施
        /// </summary>
        public string Measures { get; set; }

        /// <summary>
        /// 解决版本
        /// </summary>
        public string SolveVersion { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public IList<AttachmentList> Attachments { get; set; }

        public IssueStatusDto()
        {
            Operations = new List<IssueOperationDto>();
            Operations.Add(new IssueOperationDto());
            Attachments = new List<AttachmentList>();
            Attachments.Add(new AttachmentList());
        }
    }

    public class IssueOperationDto
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 操作记录
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }
    }

    public class AttachmentList
    {
        /// <summary>
        /// 附件名
        /// </summary>
        public string AttachmentName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string AttachmentUrl { get; set; }
    }
}