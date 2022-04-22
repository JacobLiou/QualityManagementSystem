using System.ComponentModel;

namespace QMS.Core.Enum
{
    public enum EnumPrototype
    {
        [Description("样机")]
        Prototype = 0,

        [Description("评估")]
        Assessment,

        [Description("抽检")]
        Sampling,

        [Description("安规认证")]
        SafetyCertification,

        [Description("器件替换")]
        DeviceReplace,
    }
}
