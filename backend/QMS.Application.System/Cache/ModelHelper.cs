using Furion;
using Furion.Extras.Admin.NET;
using Furion.JsonSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    internal static partial class ModelHelper
    {
        /// <summary>
        /// 根据人员ID获取人员名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetUserNameById(this long id)
        {
            var empService = App.GetService<ISsuEmpService>();
            var name = empService.GetUserList(new List<long>() { id }).Result.Values.FirstOrDefault()?.Name;
            return name;
        }

        /// <summary>
        /// 根据人员ID列表回去人员列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SysUser> GetUserListById(this IEnumerable<long> id)
        {
            var empService = App.GetService<ISsuEmpService>();
            var list = empService.GetUserList(id).Result.Values.ToList();
            return list;
        }

        /// <summary>
        /// 根据人员ID获取人员名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetProjectNameById(this long id)
        {
            var projectService = App.GetService<ISsuProjectService>();
            var name = projectService.GetProjectList(new List<long>() { id }).Result.Values.FirstOrDefault()?.ProjectName;
            return name;
        }
    }
}