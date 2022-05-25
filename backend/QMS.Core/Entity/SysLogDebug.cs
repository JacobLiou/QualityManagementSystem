using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Entity
{
    /// <summary>
    /// 调试日志表
    /// </summary>
    [Table("sys_log_debug")]
    [Comment("调试日志表")]
    public class SysLogDebug : EntityBase
    {
        /// <summary>
        /// debug类名
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// debug 方法名
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 调式点名称
        /// </summary>
        public string DebugName { get; set; }
        /// <summary>
        /// 调式点内容
        /// </summary>
        public string DebugContext { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

    }
}
