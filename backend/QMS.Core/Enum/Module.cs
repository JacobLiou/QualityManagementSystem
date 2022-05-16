using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 模块
    /// </summary>
    public enum EnumModule
    {
        [Description("研发")]
        R_D = 0,

        [Description("测试")]
        Test = 1,

        [Description("试产")]
        TrialProduce = 2,

        [Description("IQC")]
        IQC = 3,

        [Description("量产")]
        MassProduction = 4,

        [Description("售后")]
        AfterSales = 5,
    }
}
