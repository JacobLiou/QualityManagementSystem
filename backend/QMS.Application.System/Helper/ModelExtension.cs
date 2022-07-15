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
    internal static partial class ModelExtension
    {
        /// <summary>
        /// 根据人员ID获取人员名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetUserNameById(this long id)
        {
            var empService = App.GetService<ISsuEmpService>();
            var name = empService.GetUserList(new List<SsuUserInput>()
            {
                new SsuUserInput()
                {
                    Id=id
                }
            }).Result.FirstOrDefault()?.Name;
            return name;
        }

        /// <summary>
        /// 根据人员ID列表获取人员列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SysUser> GetUserListById(this IEnumerable<long> id)
        {
            var empService = App.GetService<ISsuEmpService>();
            var list = empService.GetUserList(id.Select(u => new SsuUserInput() { Id = u })).Result.ToList();
            return list;
        }

        /// <summary>
        /// 根据项目ID获取项目名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetProjectNameById(this long id)
        {
            var projectService = App.GetService<ISsuProjectService>();
            var name = projectService.GetProjectList(new List<UpdateSsuProjectInput>()
            {
                new UpdateSsuProjectInput()
                {
                    Id=id
                }
            }).Result.FirstOrDefault()?.ProjectName;
            return name == null ? "" : name;
        }

        /// <summary>
        /// 根据产品ID获取产品名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetProductNameById(this long id)
        {
            var productService = App.GetService<ISsuProductService>();
            //var name = productService.GetProductList(new List<long>() { id }).Result.Values.FirstOrDefault()?.ProductName;
            var name = productService.GetProductList(new List<UpdateSsuProductInput>()
            {
                new UpdateSsuProductInput()
                {
                    Id=id
                }
            }).Result.FirstOrDefault()?.ProductName;
            return name == null ? "" : name;
        }
    }
}