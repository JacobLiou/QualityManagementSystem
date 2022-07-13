namespace QMS.Application.Issues.Helper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotToModelPropertyAttribute : Attribute
    {
    }

    /// <summary>
    /// ListView/DataGrid修改标题名
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnHeaderAttribute : Attribute
    {
        public string HeaderName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotToTableColumnAttribute : Attribute
    {
    }

    /// <summary>
    /// 标记此特性的属性在使用ViewExtendMethod.AddWithoutTemplateByNotShowInView时不会显示在datagrid中
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotShowInViewAttribute : Attribute
    {
    }

    /// <summary>
    /// 标记此特性的属性在使用ViewExtendMethod.AddWithoutTemplateByNotShowInView时不会显示在datagrid中
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotShowInDataGridAttribute : Attribute
    {
    }

    /// <summary>
    /// 标记此特性的属性ViewExtendMethod.AddWithoutTemplateByNotShowInView时可以被编辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CanEditAttribute : Attribute
    {
    }

    #region object转dataTable

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ColumnHeadAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnHeadAttribute(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ColumnValueAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnValueAttribute(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ColumnExcelNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnExcelNameAttribute(string name)
        {
            this.Name = name;
        }
    }

    #endregion object转dataTable
}