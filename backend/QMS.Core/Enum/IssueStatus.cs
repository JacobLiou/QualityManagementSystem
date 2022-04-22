using System.ComponentModel;

namespace QMS.Core.Enum
{
    public enum EnumIssueStatus
    {
        [Description("已开启")]
        Created = 0,

        [Description("已分派")]
        Dispatched = 1,

        [Description("已处理")]
        Solved = 2,

        [Description("未解决")]
        UnSolve = 3,

        [Description("已关闭")]
        Closed,

        [Description("已挂起")]
        HasHangUp,
    }
}
