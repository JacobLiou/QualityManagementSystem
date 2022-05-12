using Furion;
using Furion.Extras.Admin.NET;
using QMS.Application.Issues.Service;
using QMS.Core.Enum;
using System.ComponentModel;
using System.Reflection;

namespace QMS.Application.Issues.Helper
{
    internal static partial class Helper
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

        #region 获取列名集合
        public static async  Task<KeyValuePair<string, string>[]> GetColumns()
        {
            return await App.GetService<IssueCacheService>().GetUserColumns(CurrentUserInfo.UserId);
        }

        //public async Task SetColumns(KeyValuePair<string, string>[] columns)
        //{
        //    var cacheKey = CommonConst.CACHE_KEY_MENU + $"{userId}-{appCode}";
        //    await _cache.SetStringAsync(cacheKey, JSON.Serialize(menus));

        //    await AddCacheKey(cacheKey);
        //}
        #endregion
    }
}
