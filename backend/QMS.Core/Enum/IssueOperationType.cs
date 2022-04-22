using System.ComponentModel;

namespace QMS.Core.Enum
{
    public enum EnumIssueOperationType
    {
        [Description("新增")]
        New = 0,

        [Description("分发")]
        Dispatch,

        [Description("处理")]
        Solve,

        [Description("关闭")]
        Close,

        [Description("挂起")]
        HangUp,
    }
}
