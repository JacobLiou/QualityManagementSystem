using Furion.DataValidation;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.System
{
    /// <summary>
    /// 项目输入参数
    /// </summary>
    public class SsuProjectInput : PageInputBase
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        public virtual long DirectorId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }
    }

    /// <summary>
    /// 项目新增参数
    /// </summary>
    public class AddSsuProjectInput
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [Required(ErrorMessage = "项目名称为必填项")]
        public virtual string ProjectName { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        [Required(ErrorMessage = "项目负责人为必填项")]
        public virtual long DirectorId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序值为必填项")]
        [DataValidation(SystemValidationTypes.NumberUnNegativeNumber, ErrorMessage = "排序值应大于等于0")]
        public virtual int Sort { get; set; }
    }

    public class DeleteSsuProjectInput : BaseId
    {
    }

    public class UpdateSsuProjectInput : SsuProjectInput
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        [Required(ErrorMessage = "项目编号不能为空")]
        public long Id { get; set; }
    }

    public class QueryeSsuProjectInput : BaseId
    {
    }
}