using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 问题状态
    /// </summary>
    public enum EnumIssueStatus
    {
        /// <summary>
        /// 已开启
        /// </summary>
        [Description("已开启")]
        Created = 0,

        /// <summary>
        /// 已分派
        /// </summary>
        [Description("已分派")]
        Dispatched = 1,

        /// <summary>
        /// 已处理
        /// </summary>
        [Description("已处理")]
        Solved = 2,

        /// <summary>
        /// 未解决
        /// </summary>
        [Description("未解决")]
        UnSolve = 3,

        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed,

        /// <summary>
        /// 已挂起
        /// </summary>
        [Description("已挂起")]
        HasHangUp,

        /// <summary>
        /// 已暂存
        /// </summary>
        [Description("已暂存")]
        HasTemporary,

        /// <summary>
        /// 已复核
        /// </summary>
        [Description("已复核")]
        HasRechecked
    }

    /// <summary>
    /// 问题回归验证状态
    /// </summary>
    public enum ValidationStatus
    {
        /// <summary>
        /// 未验证
        /// </summary>
        [Description("未验证")]
        UnVerification = 0,

        /// <summary>
        /// 验证不通过
        /// </summary>
        [Description("验证不通过")]
        DisVerification = 1,

        /// <summary>
        /// 验证通过
        /// </summary>
        [Description("验证通过")]
        Verification = 2,
    }
}