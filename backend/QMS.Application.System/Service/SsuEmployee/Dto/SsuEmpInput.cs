using Furion.Extras.Admin.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    /// <summary>
    ///
    /// </summary>
    public class SsuOrgUserInput : PageInputBase
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        [Required(ErrorMessage = "请输入部门ID")]
        public long orgId { get; set; }
    }
}