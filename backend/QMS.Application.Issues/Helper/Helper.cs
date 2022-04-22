using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace QMS.Application.Issues.Helper
{
    internal static class Helper
    {
        #region 枚举映射
        public static string GetEnumDescription<T>(this T enumInstance) where T : Enum
        {
            Type type = typeof(T);
            string name = Enum.GetName(type, enumInstance);
            var info = type.GetField(name);

            var attribute = info.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null)
            {
                return attribute.Description;
            }

            return name;
        }
        #endregion

        public static void Assert(bool result, string errorMsg="参数错误")
        {
            if (!result)
            {
                throw new ArgumentException(errorMsg);
            }
        }

        #region 项目和产品
        public static string GetNameByProjectId(this long id)
        {
            Assert(false);

            return string.Empty;
        }

        public static string GetNameByProductId(this long id)
        {
            Assert(false);

            return string.Empty;
        }
        #endregion

        #region 工号和姓名
        public static string GetNameByEmpId(this long? id)
        {
            Assert(false);

            return string.Empty;
        }

        public static string GetNameByEmpId(this long id)
        {
            Assert(false);

            return string.Empty;
        }
        #endregion

        #region 日期转换
        public static DateTime GetDateTime(this string time)
        {
            Assert(time != null);

            return Convert.ToDateTime(time);
        }

        public static string GetDateString(this DateTime time)
        {
            Assert(time != null);

            return time.ToString("YYYY-MM-dd");
        }

        public static string GetDateString(this DateTime? time)
        {
            Assert(time != null);

            return time?.ToString("YYYY-MM-dd");
        }

        public static string GetTimeString(this DateTime time)
        {
            Assert(time != null);

            return time.ToString("YYYY-MM-dd HH:mm:ss");
        }

        public static string GetTimeString(this DateTime? time)
        {
            Assert(time != null);

            return time?.ToString("YYYY-MM-dd HH:mm:ss");
        }
        #endregion
    }
}
