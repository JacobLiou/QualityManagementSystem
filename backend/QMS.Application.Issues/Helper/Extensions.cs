using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace QMS.Application.Issues.Helper
{
    public static class Extensions
    {
        public static string GetTableName<T>(this Type modelType)
        {
            if (!modelType.IsDefined(typeof(TableAttribute)))
                return modelType.Name;

            var tableNameAttr =
                (CommentAttribute)modelType.GetCustomAttribute(typeof(CommentAttribute));

            return string.IsNullOrEmpty(tableNameAttr.Comment)
                ? modelType.Name.ToPascal()
                : tableNameAttr.Comment;
        }

        /// <summary>
        /// 获取在该属性上标记的字符串， 用来修改列标题 ColumnHeaderAttribute
        /// </summary>
        public static string GetHeader(this PropertyInfo property)
        {
            if (!property.IsDefined(typeof(ColumnHeaderAttribute)))
                return property.Name;

            var attr =
                (ColumnHeaderAttribute)property.GetCustomAttribute(typeof(ColumnHeaderAttribute));

            return attr.HeaderName;
        }

        /// <summary>
        /// 用该特性填入数据库列名
        /// 将属性与数据库列映射
        /// 使用ColumnAttribute特性时 如果填入字符就是用，不填就获取Pascal命名
        /// </summary>
        public static string GetRealColumn(this PropertyInfo prop, DataTable table = null)
        {
            string name = prop.Name;

            if (table != null && table.Columns.Contains(name))
            {
                return name;
            }

            if (prop.IsDefined(typeof(CommentAttribute)))
            {
                CommentAttribute columnNameAttr =
                    prop.GetCustomAttribute(typeof(CommentAttribute)) as CommentAttribute;

                return string.IsNullOrEmpty(columnNameAttr?.Comment)
                    ? prop.Name.GetPascalFromDbField()
                    : columnNameAttr.Comment;
            }

            return name;
        }

        public static string GetPascalFromDbField(this string columnName)
        {
            string[] arrs = columnName.Split('_');

            if (arrs.Length == 1)
            {
                columnName = ToPascal(columnName);
            }
            else
            {
                columnName = string.Empty;

                foreach (var item in arrs)
                {
                    columnName += ToPascal(item);
                }
            }

            return columnName;
        }

        public static string ToPascal(this string str)
        {
            str = str.Replace("_", "").Replace("-", "");

            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
    }
}
