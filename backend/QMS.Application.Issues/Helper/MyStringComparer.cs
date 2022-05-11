using System.Diagnostics.CodeAnalysis;

namespace QMS.Application.Issues.Helper
{
    internal class MyStringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode([DisallowNull] string obj)
        {
            return obj.GetHashCode();
        }
    }
}
