using MiniExcelLibs.Attributes;

namespace QMS.Application.Issues.Service.SsuIssue.Dto.Add
{
    public class InIssue: AddToCommonIssue
    {
        /// <summary>
        /// 详情
        /// </summary>
        [ExcelColumnName("详情")]
        public string Description { get; set; }

        /// <summary>
        /// [{"IssueId":284932473958469,"AttachmentId":285613677277253,"FileName":"285612418437189.pdf","AttachmentType":0}]
        /// </summary>
        [ExcelIgnore]
        public string Attachments { get; set; }

        /// <summary>
        /// [{"IssueId":284932473958469,"AttributeId":285613677277253,"AttributeCode":"code","Value":"数据","ValueType":"string"}]
        /// </summary>
        [ExcelIgnore]
        public string ExtendAttribute { get; set; }
    }
}
