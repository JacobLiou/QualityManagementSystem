﻿using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.System
{
    /// <summary>
    /// 人员组输入参数
    /// </summary>
    public class SsuGroupInput : PageInputBase
    {
        /// <summary>
        /// 人员组名称
        /// </summary>
        public virtual string GroupName { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }
        
    }

    public class AddSsuGroupInput : SsuGroupInput
    {
    }

    public class DeleteSsuGroupInput : BaseId
    {
    }

    public class UpdateSsuGroupInput : SsuGroupInput
    {
        /// <summary>
        /// Id主键
        /// </summary>
        [Required(ErrorMessage = "Id主键不能为空")]
        public long Id { get; set; }
        
    }

    public class QueryeSsuGroupInput : BaseId
    {

    }
}