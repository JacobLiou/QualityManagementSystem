using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 生产-工序
    /// </summary>
    public enum EnumProductionProcess
    {
        [Description("ASS")]
        ASS = 0,

        [Description("DIP")]
        DIP,

        [Description("SMT")]
        SMT,

        [Description("TEST")]
        TEST,

        [Description("老化")]
        老化,

        [Description("气密")]
        气密,

        [Description("装配")]
        装配,
    }
}
