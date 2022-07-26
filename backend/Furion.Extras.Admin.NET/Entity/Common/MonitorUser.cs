using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furion.Extras.Admin.NET.Entity.Common
{
    [Table("qms_monitor_user")]
    [Comment("上位机用户")]
    public class MonitorUser : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        [Comment("用户名称")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 注册码
        /// </summary>
        [Comment("注册码")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 机器码
        /// </summary>
        [Comment("机器码")]
        [MaxLength(200)]
        [Required]
        public string Machine { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}
