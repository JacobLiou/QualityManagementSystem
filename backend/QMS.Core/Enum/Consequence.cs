using System.ComponentModel;

namespace QMS.Core.Enum
{
    public enum EnumConsequence
    {
        [Description("致命")]
        Deadly = 0,

        [Description("严重")]
        Serious,

        [Description("一般")]
        General,

        [Description("提示")]
        Prompt,
    }
}
