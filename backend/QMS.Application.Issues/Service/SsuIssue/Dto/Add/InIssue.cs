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
        /// Json ["url1","url2"]
        /// </summary>
        [ExcelIgnore]
        public string Attachments { get; set; }

        /// <summary>
        /// List<FieldValue>.ToJson()
        /// </summary>
        [ExcelIgnore]
        public string ExtendAttribute { get; set; }
    }
}
