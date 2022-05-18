using Furion.Extras.Admin.NET.Service;

namespace QMS.Application.Issues.Service.Issue.Dto.Query
{
    public class OutputDetailIssue : BaseId
    {
        /// <summary>
        /// 解决版本
        /// </summary>
        public string SolveVersion { get; set; }
        /// <summary>
        /// 验证情况
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 验证数量
        /// </summary>
        public int? Count { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 问题详情
        /// </summary>
        public string Description { get; set; }

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
        public string ExtendAttribute { get; set; }

        /// <summary>
        /// 解决措施
        /// </summary>
        public string Measures { get; set; }
        /// <summary>
        /// 挂起原因
        /// </summary>
        public string HangupReason { get; set; }
        /// <summary>
        /// 原因分析
        /// </summary>
        public string Reason { get; set; }
    }
}
