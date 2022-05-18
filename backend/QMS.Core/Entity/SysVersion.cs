using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core
{
    /// <summary>
    /// 版本表
    /// </summary>
    [Table("sys_version")]
    [Comment("版本表")]
    public class SysVersion : DEntityBase
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [Comment("版本号")]
        [Required, MaxLength(30)]
        public string Version { get; set; }
    }
}