using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 操作类型，用于操作记录
    /// </summary>
    public enum EnumIssueOperationType
    {
        [Description("新增")]
        New = 0,

        [Description("分发")]
        Dispatch,

        [Description("处理")]
        Solve,

        [Description("验证不通过")]
        NoPass,

        [Description("关闭")]
        Close,

        [Description("挂起")]
        HangUp,

        [Description("编辑")]
        Edit,

        [Description("重分发")]
        ReDispatch,

        [Description("上传文件")]
        Upload
    }
}
