using System.ComponentModel;

namespace Furion.Extras.Admin.NET.Entity.Common.Enum
{
    public enum EnumAttachmentType
    {
        [Description("普通附件")]
        NormalAttachment = 0,

        [Description("问题详情")]
        Detail,

        [Description("原因分析")]
        Reason,

        [Description("解决措施")]
        Measures,

        [Description("验证情况")]
        VertifyResult,
    }
}
