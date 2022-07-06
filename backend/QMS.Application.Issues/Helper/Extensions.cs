using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

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
            if (!property.IsDefined(typeof(CommentAttribute)))
                return property.Name;

            var attr =
                (CommentAttribute)property.GetCustomAttribute(typeof(CommentAttribute));

            return attr.Comment;
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string FormatRichText(this string txt)
        {
            //移除标签
            txt = Regex.Replace(txt, "<style[^>]*?>[\\s\\S]*?<\\/style>", "");//删除css
            txt = Regex.Replace(txt, "<script[^>]*?>[\\s\\S]*?<\\/script>", "");//删除js
            txt = Regex.Replace(txt, "<[^>]+>", "");//删除html标记
            txt = Regex.Replace(txt, "\\s*|\t|\r|\n", "");//去除tab、空格、空行
            txt = Regex.Replace(txt, "&nbsp;", "");
            txt = txt.Replace(" ", "");
            txt = txt.Replace("\"", "");//去除异常的引号" " "
            txt = txt.Replace("\"", "");
            //移除多余属性
            txt = Regex.Replace(txt, "<source.*?>", "");
            txt = Regex.Replace(txt, "<video.*?>", "");
            txt = Regex.Replace(txt, "</video>", "");
            txt = Regex.Replace(txt, "class[^=]*=[\"']*[^\"'>]+[\"']*", "");
            txt = Regex.Replace(txt, "style[^=]*=[\"']*[^\"'>]+[\"']*", "");
            txt = Regex.Replace(txt, "width[^=]*=[\"']*[^\"'>]+[\"']*", "");
            txt = Regex.Replace(txt, "height[^=]*=[\"']*[^\"'>]+[\"']*", "");
            txt = Regex.Replace(txt, "href[^=]*=[\"']*[^\"'>]+[\"']*", "");//去除a标签 href
            txt = Regex.Replace(txt, "<style[^>]*?>[\\s\\S]*?<\\/style>", "");//去除style
            txt = Regex.Replace(txt, "<script[^>]*?>[\\s\\S]*?<\\/script>", "");//去除script
            txt = Regex.Replace(txt, "&nbsp;", "");
            txt = Regex.Replace(txt, "<p></p>", "");
            txt = Regex.Replace(txt, "figure", "p");
            return txt;
        }
    }
}