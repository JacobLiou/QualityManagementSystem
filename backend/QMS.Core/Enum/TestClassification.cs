using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 测试类别
    /// </summary>
    public enum EnumTestClassification
    {
        /// <summary>
        /// 整机
        /// </summary>
        [Description("整机")]
        Whole = 0,
        [Description("单板")]
        Single,
        [Description("安规")]
        Safe,
    }
}
