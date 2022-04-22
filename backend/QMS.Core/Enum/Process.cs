using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 工序
    /// </summary>
    public enum EnumProcess
    {
        [Description("SMT-锡膏印刷")]
        SMT_锡膏印刷 = 0,

        [Description("SMT-贴片")]
        SMT_贴片,

        [Description("SMT-点胶")]
        SMT_点胶,

        [Description("SMT-目检")]
        SMT_目检,

        [Description("DIP-前加工")]
        DIP_前加工 = 0,

        [Description("DIP-插件")]
        DIP_插件,

        [Description("DIP-补焊")]
        DIP_补焊,

        [Description("DIP-波峰焊接")]
        DIP_波峰焊接,

        [Description("TEST-FT单板")]
        TEST_FT单板,

        [Description("老化")]
        老化,

        [Description("包装")]
        包装,
    }
}
