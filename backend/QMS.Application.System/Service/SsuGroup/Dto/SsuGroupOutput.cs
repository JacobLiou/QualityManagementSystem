﻿using Furion.Extras.Admin.NET.Service;
using System;

namespace QMS.Application.System
{
    /// <summary>
    /// 人员组输出参数
    /// </summary>
    public class SsuGroupOutput
    {
        /// <summary>
        /// 人员组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// Id主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 关联人员列表
        /// </summary>
        public IEnumerable<UserOutput> UserList { get; set; }
    }
}