using Furion;
using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET;
using Furion.JsonSerialization;
using QMS.Core;
using QMS.Core.Entity;
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
            if (id == null)
            {
                return string.Empty;
            }

            return "员工" + id?.ToString();
        }

        public static string GetNameByEmpId(this long id)
        {
            //Assert(false);

            return "员工" + id.ToString();
        }
        #endregion

        #region 缓存获取列名集合
        public static async Task<Dictionary<string, string>> GetUserColumns(IRepository<IssueColumnDisplay, IssuesDbContextLocator> columnDisplayRepository)
        {
            long userId = CurrentUserInfo.UserId;

            var cacheService = App.GetService<IssueCacheService>();

            string cacheString = await cacheService.GetUserColumns(userId);

            // 如果缓存没有，就尝试从数据取并放到缓存，最终取缓存
            if (string.IsNullOrEmpty(cacheString))
            {
                var model = columnDisplayRepository?.DetachedEntities.FirstOrDefault(column => column.UserId == userId);
                if (model != null)
                {
                    await SetUserColumns(columnDisplayRepository, model.Columns);
                }
                else
                {
                    string jsonStr = JSON.Serialize(Constants.USER_COLUMN_NAMES);
                    await columnDisplayRepository.InsertNowAsync(new IssueColumnDisplay
                    {
                        UserId = CurrentUserInfo.UserId,
                        Columns = jsonStr
                    });

                    await columnDisplayRepository.Context.SaveChangesAsync();

                    await cacheService.SetUserColumns(CurrentUserInfo.UserId, jsonStr);
                }
            }

            string json = await cacheService.GetUserColumns(userId);

            return JSON.Deserialize<Dictionary<string, string>>(json);
        }

        public static async Task SetUserColumns(
            IRepository<IssueColumnDisplay, IssuesDbContextLocator> columnDisplayRepository,
            string jsonStr
        )
        {
            await App.GetService<IssueCacheService>().SetUserColumns(CurrentUserInfo.UserId, jsonStr);

            await columnDisplayRepository.UpdateNowAsync(new IssueColumnDisplay
            {
                UserId = CurrentUserInfo.UserId,
                Columns = jsonStr
            });

            await columnDisplayRepository.Context.SaveChangesAsync();
        }
        #endregion
    }
}
