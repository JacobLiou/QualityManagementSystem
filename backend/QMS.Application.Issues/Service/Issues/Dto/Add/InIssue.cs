using MiniExcelLibs.Attributes;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.Service.Issue.Dto.Add
{
    public class InIssue: AddToCommonIssue
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
        /// issueId：问题编号 long
        /// attributeId：字段编号  long
        /// attributeCode：字段代码 string
        /// value：字段值 string
        /// valueType：字段类型 string
        /// [{"issueId":284932473958469,"attributeId":285613677277253,"attributeCode":"code","value":"数据","valueType":"string"}]
        /// </summary>
        [ExcelIgnore]
        //[Required]
        public string ExtendAttribute { get; set; }
    }
}
