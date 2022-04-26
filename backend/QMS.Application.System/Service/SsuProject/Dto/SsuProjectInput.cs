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

    public class AddSsuProjectInput : SsuProjectInput
    {
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
