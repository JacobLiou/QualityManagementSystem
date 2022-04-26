using System;
using Furion.Extras.Admin.NET;

namespace QMS.Application.System
{
    /// <summary>
    /// 项目输出参数
    /// </summary>
    public class SsuProjectDto
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        
        /// <summary>
        /// 项目负责人
        /// </summary>
        public long DirectorId { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 项目编号
        /// </summary>
        public long Id { get; set; }
        
    }
}
