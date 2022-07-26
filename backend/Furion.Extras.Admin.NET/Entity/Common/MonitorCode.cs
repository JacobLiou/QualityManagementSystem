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
    [Table("qms_monitor_code")]
    [Comment("上位机注册码")]
    public class MonitorCode : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 注册码
        /// </summary>
        [Comment("注册码")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 注册码
        /// </summary>
        [Comment("权限")]
        [MaxLength(50)]
        [Required]
        public string Role { get; set; }


    }
}
