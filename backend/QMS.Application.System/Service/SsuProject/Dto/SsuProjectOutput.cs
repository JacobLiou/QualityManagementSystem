using Furion.Extras.Admin.NET.Service;
using System;

namespace QMS.Application.System
{
    /// <summary>
    /// 项目输出参数
    /// </summary>
    public class SsuProjectOutput
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
        /// 项目负责人名称
        /// </summary>
        public string DirectorName { get; set; }

        /// <summary>
        /// 所属产品
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 所属产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 项目描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 关联人员列表
        /// </summary>
        public IEnumerable<UserOutput> UserList { get; set; }
    }
}