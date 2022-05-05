using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System.Service
{
    public class GroupUserOutput
    {
        /// <summary>
        /// 人员组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 人员组ID
        /// </summary>
        public string GroupID { get; set; }
        /// <summary>
        /// 用戶
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用戶ID
        /// </summary>
        public string UserID { get; set; }
    }
}
