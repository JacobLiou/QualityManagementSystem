using System.ComponentModel;

namespace QMS.Core.Enum
{
    public enum TestClassification
    {
        [Description("整机")]
        Whole = 0,
        [Description("单板")]
        Single,
        [Description("安规")]
        Safe,
    }
}
