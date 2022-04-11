namespace Furion.Extras.Admin.NET
{
    public class PageResult<T>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalRows { get; set; }
        public ICollection<T> Rows { get; set; }
    }
}