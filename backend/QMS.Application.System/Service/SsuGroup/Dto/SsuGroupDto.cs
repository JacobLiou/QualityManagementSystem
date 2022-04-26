using System;
using Furion.Extras.Admin.NET;

namespace QMS.Application.System
{
    /// <summary>
    /// 人员组输出参数
    /// </summary>
    public class SsuGroupDto
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
        
    }
}
