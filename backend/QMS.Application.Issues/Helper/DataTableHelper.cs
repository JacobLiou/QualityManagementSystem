using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{
    public static class DataTableHelper
    {
        /// <summary>
        /// 对象转DataTable，以自定义特性名作为列名，以属性值作为值，对象集合最终生成多行多列数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataTable ObjectToTable(object obj)
        {
            try
            {
                Type t;
                if (obj.GetType().IsGenericType)
                {
                    t = obj.GetType().GetGenericTypeDefinition();
                }
                else
                {
                    t = obj.GetType();
                }
                if (t == typeof(List<>) ||
                    t == typeof(IEnumerable<>))
                {
                    DataTable dt = new DataTable();
                    IEnumerable<object> lstenum = obj as IEnumerable<object>;
                    if (lstenum.Count() > 0)
                    {
                        var ob1 = lstenum.GetEnumerator();
                        ob1.MoveNext();
                        //列名
                        foreach (var item in ob1.Current.GetType().GetProperties())
                        {
                            if (item.CustomAttributes.Count() > 0)
                            {
                                var itemAttributes = item.CustomAttributes.FirstOrDefault(u => u.AttributeType.Name == "ColumnExcelNameAttribute");
                                if (itemAttributes != null)
                                {
                                    dt.Columns.Add(new DataColumn() { ColumnName = itemAttributes.ConstructorArguments.FirstOrDefault().Value.ToString() });
                                }
                            }
                        }
                        //数据
                        foreach (var item in lstenum)
                        {
                            DataRow row = dt.NewRow();
                            foreach (var sub in item.GetType().GetProperties())
                            {
                                if (sub.CustomAttributes.Count() > 0)
                                {
                                    var itemAttributes = sub.CustomAttributes.FirstOrDefault(u => u.AttributeType.Name == "ColumnExcelNameAttribute");
                                    if (itemAttributes != null)
                                    {
                                        row[itemAttributes.ConstructorArguments.FirstOrDefault().Value.ToString()] = sub.GetValue(item, null);
                                    }
                                }
                            }
                            dt.Rows.Add(row);
                        }
                        return dt;
                    }
                }
                else if (t == typeof(DataTable))
                {
                    return (DataTable)obj;
                }
                else   //(t==typeof(Object))
                {
                    DataTable dt = new DataTable();
                    foreach (var item in obj.GetType().GetProperties())
                    {
                        dt.Columns.Add(new DataColumn() { ColumnName = item.Name });
                    }
                    DataRow row = dt.NewRow();
                    foreach (var item in obj.GetType().GetProperties())
                    {
                        row[item.Name] = item.GetValue(obj, null);
                    }
                    dt.Rows.Add(row);
                    return dt;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// 对象转DataTable，以对象的其他一个属性当做列名，以另外一个属性当作值，对象集合最终生成一行多列数据
        /// </summary>
        /// <param name="row">要追加数据的数据行</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataTable AddObjectToTable(DataRow row, object obj)
        {
            try
            {
                Type t;
                if (obj.GetType().IsGenericType)
                {
                    t = obj.GetType().GetGenericTypeDefinition();
                }
                else
                {
                    t = obj.GetType();
                }
                if (t == typeof(List<>) ||
                    t == typeof(IEnumerable<>))
                {
                    if (row == null)
                    {
                        DataTable dt = new DataTable();
                        row = dt.NewRow();
                    }
                    string columnName = "";
                    IEnumerable<object> lstenum = obj as IEnumerable<object>;
                    if (lstenum.Count() > 0)
                    {
                        foreach (var item in lstenum)
                        {
                            foreach (var sub in item.GetType().GetProperties())
                            {
                                if (sub.CustomAttributes.Count() > 0)
                                {
                                    var itemAttributes = sub.CustomAttributes.FirstOrDefault(u => u.AttributeType.Name == "ColumnHeadAttribute");
                                    if (itemAttributes != null)
                                    {
                                        if (!row.Table.Columns.Contains(sub.GetValue(item, null).ToString()))
                                        {
                                            columnName = sub.GetValue(item, null).ToString();
                                            row.Table.Columns.Add(new DataColumn() { ColumnName = columnName });
                                            break;
                                        }
                                        else
                                        {
                                            columnName = sub.GetValue(item, null).ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                            foreach (var sub in item.GetType().GetProperties())
                            {
                                if (sub.CustomAttributes.Count() > 0)
                                {
                                    var valueAttributes = sub.CustomAttributes.FirstOrDefault(u => u.AttributeType.Name == "ColumnValueAttribute");
                                    if (valueAttributes != null)
                                    {
                                        row[columnName] = sub.GetValue(item, null);
                                        break;
                                    }
                                }
                            }
                        }
                        return row.Table;
                    }
                }
                else if (t == typeof(DataTable))
                {
                    return (DataTable)obj;
                }
                else   //(t==typeof(Object))
                {
                    DataTable dt = new DataTable();
                    foreach (var item in obj.GetType().GetProperties())
                    {
                        dt.Columns.Add(new DataColumn() { ColumnName = item.Name });
                    }
                    //DataRow row = dt.NewRow();
                    foreach (var item in obj.GetType().GetProperties())
                    {
                        row[item.Name] = item.GetValue(obj, null);
                    }
                    dt.Rows.Add(row);
                    return dt;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}