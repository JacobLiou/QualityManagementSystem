using System;
using Furion.Extras.Admin.NET;

namespace QMS.Application.System
{
    /// <summary>
    /// 人员组关联组织架构
    /// </summary>
    public class SsuEmpOrg
    {

    }

    /// <summary>
    /// 人员组关联成员
    /// </summary>
    public class SsuEmpGroup
    {
        /// <summary>
        /// 人员组Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 人员组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 成员的姓名
        /// </summary>
        public string EmpName { get; set; }
    }

    /// <summary>
    /// 人员组关联项目
    /// </summary>
    public class SsuEmpProject
    {

    }
}
