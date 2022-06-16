using Furion.Extras.Admin.NET;
using MiniExcelLibs.Attributes;
using QMS.Core.Enum;

namespace QMS.Application.Issues.Service.Issue.Dto.Add
{
    public class InIssue : AddToCommonIssue
    {
        /// <summary>
        /// 详情
        /// </summary>
        [ExcelColumnName("详情")]
        [ExcelColumnIndex(1)]
        [ExcelColumnWidth(50)]
        public string Description { get; set; }

        /// <summary>
        /// 用于查询该问题有哪些附件
        /// [{"AttachmentId":287372965613637,"FileName":"20220511164546.xlsx","AttachmentType":0}]
        /// </summary>
        //[ExcelIgnore]
        //public string Attachments { get; set; }

        /// <summary>
        /// 用于新增和分发时保存动态生成的字段信息（动态生成对应控件时,字段结构可通过相应接口获得）
        /// module：模块 int
        /// issueId：问题编号 long
        /// fieldId：字段编号  long
        /// fieldCode：字段代码 string
        /// fieldName：字段代码的中文意思 string
        /// value：字段值 string
        /// valueType：字段类型 string
        /// [{"module": 0, "issueId":284932473958469,"fieldId":285613677277253,"fieldCode":"code", "fieldName":"中文代码", "value":"数据","fieldDataType":"string"}]
        /// </summary>
        [ExcelIgnore]
        public string ExtendAttribute { get; set; }

        public bool SetIssuse(Core.Entity.Issue issue)
        {
            issue.CreateTime = DateTime.Now;
            issue.CreatorId = CurrentUserInfo.UserId;

            if (this.IsTemporary)
            {
                this.Status = EnumIssueStatus.HasTemporary;
            }
            else
            {
                this.Status = EnumIssueStatus.Created;
            }
            return true;
        }
    }
}