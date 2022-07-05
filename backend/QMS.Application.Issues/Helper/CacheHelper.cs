using Furion;
using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET;
using Furion.JsonSerialization;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using QMS.Application.Issues.Field;
using QMS.Application.Issues.Service.ThirdPartyService.Dto;
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
                typeof(EnumProcessType),
                typeof(EnumProductionProcess),
                typeof(EnumTrialProductionProcess),
                typeof(EnumVeneer),
                typeof(EnumPrototype),
                typeof(EnumTestClassification),
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
            return -1;
            //throw new Exception("导入的数据(模块,问题性质,问题分类,问题来源,问题状态)超出范围");
        }

        /// <summary>
        /// 判断枚举类型下是否存在对应的枚举值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckEnumValue<T>(dynamic value)
        {
            return Enum.IsDefined(typeof(T), value);
        }


        #endregion 枚举映射

        #region 项目和产品

        public static string GetNameByProjectId(this long id)
        {
            //Assert(false);

            var cache = App.GetService<IDistributedCache>();
            string cacheStr = cache.GetString(CoreCommonConst.PROJECTID + id);
            //缓存存在则获取缓存值
            if (!string.IsNullOrEmpty(cacheStr))
            {
                return JSON.Deserialize<ProjectModelFromThirdParty>(cacheStr).ProjectName;
            }
            //缓存不存在，则调用system下的接口获取值
            Dictionary<long, ProjectModelFromThirdParty> project = Helper.GetThirdPartyService().GetProjectByIds(new List<long>() { id }).Result;
            if (project == null || !project.Keys.Contains(id))
            {
                return "";
            }
            var projectName = project.FirstOrDefault(u => u.Key == id).Value.ProjectName;

            return projectName;
        }

        /// <summary>
        /// 通过名称获取ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static long GetProjectIdByName(this string name)
        {
            var cache = App.GetService<IDistributedCache>();
            string cacheStr = cache.GetString(CoreCommonConst.PROJECTNAME + name);
            //缓存存在则获取缓存值
            if (!string.IsNullOrEmpty(cacheStr))
            {
                return JSON.Deserialize<ProjectModelFromThirdParty>(cacheStr).Id;
            }
            Dictionary<string, ProjectModelFromThirdParty> project = Helper.GetThirdPartyService().GetProjectByNames(new List<string>() { name }).Result;
            var projectId = project.FirstOrDefault(u => u.Key == name).Value.Id;
            return projectId;
        }

        public static string GetNameByProductId(this long? id)
        {
            if (id == null)
            {
                return string.Empty;
            }

            return GetNameByProductId((long)id);

            //return "员工" + id?.ToString();
        }

        public static string GetNameByProductId(this long id)
        {
            //Assert(false);

            var cache = App.GetService<IDistributedCache>();
            string cacheStr = cache.GetString(CoreCommonConst.PRODUCTID + id);
            //缓存存在，则获取缓存值
            if (!string.IsNullOrEmpty(cacheStr))
            {
                return JSON.Deserialize<ProductModelFromThirdParty>(cacheStr).ProductName;
            }
            //缓存不存在，则调用system下的接口获取值
            Dictionary<long, ProductModelFromThirdParty> product = Helper.GetThirdPartyService().GetProductByIds(new List<long>() { id }).Result;
            if (product == null || !product.Keys.Contains(id))
            {
                return "";
            }
            var productName = product.FirstOrDefault(u => u.Key == id).Value.ProductName;

            return productName;
        }

        /// <summary>
        /// 通过产品名称获取产品ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static long GetProductIdByName(this string name)
        {
            var cache = App.GetService<IDistributedCache>();
            string cacheStr = cache.GetString(CoreCommonConst.PRODUCTNAME + name);
            //缓存存在，则获取缓存值
            if (!string.IsNullOrEmpty(cacheStr))
            {
                return JSON.Deserialize<ProductModelFromThirdParty>(cacheStr).Id;
            }
            //缓存不存在，则调用system下的接口获取值
            Dictionary<string, ProductModelFromThirdParty> product = Helper.GetThirdPartyService().GetProductByNames(new List<string>() { name }).Result;
            if (product == null || !product.Keys.Contains(name))
            {
                return 0;
            }
            var id = product.FirstOrDefault(u => u.Key == name).Value.Id;
            return id;
        }

        /// <summary>
        /// 根据项目ID和模块ID获取对应的人员列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="modularId"></param>
        /// <returns></returns>
        public static List<UserModelFromThirdParty> GetUserByProjectModularId(long projectId, long modularId)
        {
            var cache = App.GetService<IDistributedCache>();
            string cacheStr = cache.GetString(CoreCommonConst.PROJECT_MODULAR + projectId + "_" + modularId);

            //缓存存在，则获取缓存值
            if (!string.IsNullOrEmpty(cacheStr))
            {
                return JSON.Deserialize<List<UserModelFromThirdParty>>(cacheStr);
            }
            //缓存不存在，则调用system下的接口获取值
            List<UserModelFromThirdParty> userList = Helper.GetThirdPartyService().GetUserByProjectModularId(projectId, modularId).
                Result.AsQueryable().ProjectToType<List<UserModelFromThirdParty>>().FirstOrDefault();
            return userList;
        }

        #endregion 项目和产品

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

            return GetNameByEmpId((long)id);

            //return "员工" + id?.ToString();
        }

        public static string GetNameByEmpId(this long id)
        {
            //Assert(false);

            //var cacheService = App.GetService<IssueCacheService>();

            //string cacheStr = cacheService.GetUserName(id).Result;

            var cache = App.GetService<IDistributedCache>();
            string cacheStr = cache.GetString(CoreCommonConst.USERID + GetTenantId() + "_" + id);

            //缓存存在则获取缓存值
            if (!string.IsNullOrEmpty(cacheStr))
            {
                return JSON.Deserialize<UserModelFromThirdParty>(cacheStr).Name;
            }
            //缓存不存在则调用system下的接口获取值
            string name = Helper.GetThirdPartyService().GetNameById(id).Result;

            return name;
        }

        public static long GetEmpIdByName(this string name)
        {
            var cache = App.GetService<IDistributedCache>();
            string cacheStr = cache.GetString(CoreCommonConst.USERNAME + GetTenantId() + "_" + name);
            //缓存存在则获取缓存值
            if (!string.IsNullOrEmpty(cacheStr))
            {
                return JSON.Deserialize<UserModelFromThirdParty>(cacheStr).Id;
            }
            //缓存不存在则调用system下的接口获取值
            long id = Helper.GetThirdPartyService().GetUserByName(new List<string>() { name }).Result.Values.FirstOrDefault().Id;

            return id;
        }

        #endregion 工号和姓名

        #region 模块相关

        public static long GetModularIdByValue(this string value)
        {
            var cache = App.GetService<IDistributedCache>();
            string cacheStr = cache.GetString(CoreCommonConst.MODULARVALUE + value);

            //缓存存在则获取缓存值
            if (!string.IsNullOrEmpty(cacheStr))
            {
                return JSON.Deserialize<ModularModelFromThirdParty>(cacheStr).Id;
            }
            //缓存不存在则调用system下的接口获取值
            long id = Helper.GetThirdPartyService().GetModularByValue(new List<string>() { value }).Result.Values.FirstOrDefault().Id;
            return id;
        }

        #endregion 模块相关

        public static string GetTenantId()
        {
            if (App.User == null) return string.Empty;
            return App.User.FindFirst(ClaimConst.TENANT_ID)?.Value + "_";
        }

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
                    cacheString = JSON.Serialize(Constants.USER_COLUMN_NAMES);
                    await columnDisplayRepository.InsertNowAsync(new IssueColumnDisplay
                    {
                        UserId = CurrentUserInfo.UserId,
                        Columns = cacheString
                    });

                    await columnDisplayRepository.Context.SaveChangesAsync();

                    await cacheService.SetUserColumns(Helper.GetCurrentUser(), cacheString);
                }
            }

            return JSON.Deserialize<Dictionary<string, string>>(cacheString);
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

        #endregion 缓存获取列名集合

        #region 缓存扩展字段结构信息

        public static async Task<Dictionary<string, FieldStruct>> GetFieldsStruct(IRepository<IssueExtendAttribute, IssuesDbContextLocator> issueExtAttrRepository)
        {
            var cacheService = App.GetService<IssueCacheService>();

            Dictionary<string, FieldStruct> cacheDic = null;
            var cacheString = await cacheService.GetFieldsStruct();

            // 如果缓存没有，就尝试从数据取并放到缓存，最终取缓存
            if (string.IsNullOrEmpty(cacheString))
            {
                lock (typeof(Helper))
                {
                    if (string.IsNullOrEmpty(cacheString))
                    {
                        cacheDic = issueExtAttrRepository.DetachedEntities.Select<IssueExtendAttribute, FieldStruct>(model => new FieldStruct()
                        {
                            FieldId = model.Id,
                            FieldCode = model.AttributeCode,
                            FieldName = model.AttibuteName,
                            FieldDataType = model.ValueType,
                            Module = model.Module
                        }).ToDictionary<FieldStruct, string>(item => item.Module.ToString().ToLower() + "_" + item.FieldCode);

                        cacheString = JSON.Serialize(cacheDic);

                        cacheService.SetFieldsStruct(cacheString);
                    }
                }
            }

            if (cacheDic == null)
            {
                cacheDic = JSON.Deserialize<Dictionary<string, FieldStruct>>(cacheString);
            }

            return cacheDic;
        }

        #endregion 缓存扩展字段结构信息
    }
}