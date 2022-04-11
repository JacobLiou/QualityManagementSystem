using Furion.FriendlyException;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Furion.Extras.Admin.NET
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumUtil
    {
        // 枚举显示字典缓存
        private static readonly ConcurrentDictionary<Type, Dictionary<int, string>> EnumDisplayValueDict = new();

        // 枚举值字典缓存
        private static readonly ConcurrentDictionary<Type, Dictionary<int, string>> EnumNameValueDict = new();

        // 枚举类型缓存
        private static ConcurrentDictionary<string, Type> _enumTypeDict = null;

        /// <summary>
        /// 获取枚举对象Key与名称的字典（缓存）
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumDictionary(Type enumType)
        {
            if (!enumType.IsEnum)
                throw Oops.Oh(ErrorCode.D1503);

            // 查询缓存
            Dictionary<int, string> enumDic = EnumNameValueDict.ContainsKey(enumType) ? EnumNameValueDict[enumType] : new Dictionary<int, string>();
            if (enumDic.Count == 0)
            {
                // 取枚举类型的Key/Value字典集合
                enumDic = GetEnumDictionaryItems(enumType);

                // 缓存
                EnumNameValueDict[enumType] = enumDic;
            }
            return enumDic;
        }

        /// <summary>
        /// 获取枚举对象Key与名称的字典
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private static Dictionary<int, string> GetEnumDictionaryItems(Type enumType)
        {
            // 获取类型的字段，初始化一个有限长度的字典
            FieldInfo[] enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            Dictionary<int, string> enumDic = new(enumFields.Length);

            // 遍历字段数组获取key和name
            foreach (FieldInfo enumField in enumFields)
            {
                int intValue = (int)enumField.GetValue(enumType);
                enumDic[intValue] = enumField.Name;
            }
            return enumDic;
        }

        /// <summary>
        /// 获取枚举类型key与描述的字典（缓存）
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<int, string> GetEnumDescDictionary(Type enumType)
        {
            if (!enumType.IsEnum)
                throw Oops.Oh(ErrorCode.D1503);

            // 查询缓存
            Dictionary<int, string> enumDic = EnumDisplayValueDict.ContainsKey(enumType) ? EnumDisplayValueDict[enumType] : new Dictionary<int, string>();
            if (enumDic.Count == 0)
            {
                // 取枚举类型的Key/Value字典集合
                enumDic = GetEnumDescDictionaryItems(enumType);

                // 缓存
                EnumDisplayValueDict[enumType] = enumDic;
            }
            return enumDic;
        }

        /// <summary>
        /// 获取枚举类型key与描述的字典（没有描述则获取name）
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Dictionary<int, string> GetEnumDescDictionaryItems(Type enumType)
        {
            // 获取类型的字段，初始化一个有限长度的字典
            FieldInfo[] enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            Dictionary<int, string> enumDic = new(enumFields.Length);

            // 遍历字段数组获取key和name
            foreach (FieldInfo enumField in enumFields)
            {
                int intValue = (int)enumField.GetValue(enumType);
                var desc = enumField.GetDescriptionValue<DescriptionAttribute>();
                enumDic[intValue] = desc != null && !string.IsNullOrEmpty(desc.Description) ? desc.Description : enumField.Name;
            }
            return enumDic;
        }

        /// <summary>
        /// 从程序集中查找指定枚举类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type TryToGetEnumType(Assembly assembly, string typeName)
        {
            // 枚举缓存为空则重新加载枚举类型字典
            _enumTypeDict ??= LoadEnumTypeDict(assembly);

            // 按名称查找
            if (_enumTypeDict.ContainsKey(typeName))
            {
                return _enumTypeDict[typeName];
            }
            return null;
        }

        /// <summary>
        /// 从程序集中加载所有枚举类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static ConcurrentDictionary<string, Type> LoadEnumTypeDict(Assembly assembly)
        {
            // 取程序集中所有类型
            Type[] typeArray = assembly.GetTypes();

            // 过滤非枚举类型，转成字典格式并返回
            Dictionary<string, Type> dict = typeArray.Where(o => o.IsEnum).ToDictionary(o => o.Name, o => o);
            ConcurrentDictionary<string, Type> enumTypeDict = new(dict);
            return enumTypeDict;
        }

        /// <summary>
        /// 获取枚举的Description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value)
        {
            return value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }

        /// <summary>
        /// 获取枚举的Description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this object value)
        {
            return value.GetType().GetMember(value.ToString() ?? string.Empty).FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()?.Description;
        }

        /// <summary>
        /// 将枚举转成枚举信息集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type '" + type.Name + "' is not an enum.");
            var arr = System.Enum.GetNames(type);
            return arr.Select(sl =>
            {
                var item = System.Enum.Parse(type, sl);
                return new EnumEntity
                {
                    Name = item.ToString(),
                    Describe = item.GetDescription(),
                    Value = item.GetHashCode()
                };
            }).ToList();
        }

        /// <summary>
        /// 枚举ToList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> EnumToList<T>(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type '" + type.Name + "' is not an enum.");
            var arr = System.Enum.GetNames(type);
            return arr.Select(name => (T)System.Enum.Parse(type, name)).ToList();
        }
    }
}