using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET;
using Microsoft.EntityFrameworkCore;
using QMS.Core;
using QMS.Core.Entity;
using QMS.Core.Enum;
using System.ComponentModel;
using System.Reflection;

namespace QMS.Application.Issues.Helper
{
    internal static class Helper
    {
        #region 枚举映射
        public static string GetEnumDescription<T>(this T enumInstance) where T : Enum
        {
            Type type = typeof(T);

            foreach (var item in EnumUtil.GetEnumDescDictionary(type))
            {
                if (Convert.ToInt32(enumInstance) == item.Key)
                {
                    return item.Value;
                }
            }

            string name = Enum.GetName(type, enumInstance);
            var info = type.GetField(name);

            var attribute = info.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null)
            {
                return attribute.Description;
            }

            return name;
        }

        public static int GetIntFromEnumDescription(dynamic value)
        {
            KeyValuePair<int, string> pair = new KeyValuePair<int, string>(-1, "Empty");

            Type[] types = new Type[]
            {
                typeof(EnumModule),
                typeof(EnumConsequence),
                typeof(EnumIssueClassification),
                typeof(EnumIssueSource),
                typeof(EnumIssueStatus),
            };

            Dictionary<int, string> map;

            foreach (var item in types)
            {
                map = EnumUtil.GetEnumDescDictionary(item);
                if (map.ContainsValue(value))
                {
                    return map.FirstOrDefault(m => m.Value == value).Key;
                }
            }

            throw new Exception("导入的数据(模块,问题性质,问题分类,问题来源,问题状态)超出范围");
        }

        #endregion

        #region Assert

        public static async Task<SsuIssue> CheckIssueExist(IRepository<SsuIssue, IssuesDbContextLocator> ssuIssueRep, long id)
        {
            SsuIssue issue = await ssuIssueRep.DetachedEntities.FirstOrDefaultAsync(issue => issue.Id == id);

            Helper.Assert(issue != null, $"问题【{id}】不存在");

            return issue;
        }

        public static async Task<SsuIssueDetail> CheckIssueDetailExist(IRepository<SsuIssueDetail, IssuesDbContextLocator> ssuIssueDetailRep, long id)
        {
            SsuIssueDetail issue = await ssuIssueDetailRep.DetachedEntities.FirstOrDefaultAsync(issue => issue.Id == id);

            Helper.Assert(issue != null, $"问题【{id}】详情不存在");

            return issue;
        }

        public static void Assert(bool result, string errorMsg = "参数错误")
        {
            if (!result)
            {
                throw new ArgumentException(errorMsg);
            }
        }

        public static void Assert(string param, Predicate<string> predicate, string errorMsg = "参数错误")
        {
            if (predicate != null && !predicate.Invoke(param))
            {
                throw new ArgumentException(errorMsg);
            }
        }
        #endregion

        #region 项目和产品
        public static string GetNameByProjectId(this long id)
        {
            //Assert(false);

            return "项目" + id.ToString();
        }

        public static string GetNameByProductId(this long id)
        {
            //Assert(false);

            return "产品" + id.ToString();
        }
        #endregion

        #region 工号和姓名
        public static long GetCurrentUser()
        {
            return CurrentUserInfo.UserId;
        }

        public static string GetNameByEmpId(this long? id)
        {
            //if (id == null)
            //{
            //    id = Helper.GetCurrentUser();
            //}

            return "员工" + id?.ToString();
        }

        public static string GetNameByEmpId(this long id)
        {
            //Assert(false);

            return "员工" + id.ToString();
        }
        #endregion

        #region 日期转换
        public static DateTime GetDateTime(this string time)
        {
            //Assert(time != null);

            return Convert.ToDateTime(time);
        }

        public static string GetDateString(this DateTime time)
        {
            //Assert(time != null);

            return time.ToString("yyyy-MM-dd");
        }

        public static string GetDateString(this DateTime? time)
        {
            //Assert(time != null);

            return time?.ToString("yyyy-MM-dd");
        }

        public static string GetTimeString(this DateTime time)
        {
            //Assert(time != null);

            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetTimeString(this DateTime? time)
        {
            //Assert(time != null);

            return time?.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion
    }
}
