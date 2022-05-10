using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using MiniExcelLibs.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues
{
    /// <summary>
    /// 问题扩展属性输入参数
    /// </summary>
    public class SsuIssueExtendAttributeInput : PageInputBase
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        public virtual QMS.Core.Enum.EnumModule? Module { get; set; }
        
        /// <summary>
        /// 字段名
        /// </summary>
        public virtual string AttibuteName { get; set; }
        
        /// <summary>
        /// 字段代码
        /// </summary>
        public virtual string AttributeCode { get; set; }
        
        /// <summary>
        /// 字段值类型
        /// </summary>
        public virtual string ValueType { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual long CreatorId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 更新人
        /// </summary>
        public virtual long UpdateId { get; set; }
        
        /// <summary>
        /// 提出日期
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }
        
        /// <summary>
        /// 排序优先级
        /// </summary>
        public virtual int Sort { get; set; }
        
    }

    public class AddSsuIssueExtendAttributeInput
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        [ExcelColumnName("模块名")]
        [Required(ErrorMessage = "模块编号不能为空")]
        public QMS.Core.Enum.EnumModule? Module { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [ExcelColumnName("字段名")]
        [Required(ErrorMessage = "字段名不能为空")]
        public string AttibuteName { get; set; }
        
        /// <summary>
        /// 字段代码
        /// </summary>
        [ExcelColumnName("字段代码")]
        [Required(ErrorMessage = "字段代码不能为空")]
        public string AttributeCode { get; set; }

        /// <summary>
        /// 字段值类型
        /// </summary>
        [ExcelColumnName("字段值类型")]
        [Required(ErrorMessage = "字段值类型不能为空")]
        public string ValueType { get; set; }
        
    }

    public class DeleteSsuIssueExtendAttributeInput : BaseId
    {
    }

    public class UpdateSsuIssueExtendAttributeInput : SsuIssueExtendAttributeInput
    {
        /// <summary>
        /// 字段编号
        /// </summary>
        [Required(ErrorMessage = "字段编号不能为空")]
        public long Id { get; set; }
        
    }

    public class QueryeSsuIssueExtendAttributeInput : BaseId
    {

    }
}
