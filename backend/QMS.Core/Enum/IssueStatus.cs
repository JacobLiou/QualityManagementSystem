using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 问题状态
    /// </summary>
    public enum EnumIssueStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        [Description("新建")]
        Created = 0,

        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")]
        Dispatched = 1,

        /// <summary>
        /// 待复核
        /// </summary>
        [Description("待复核")]
        Solved = 2,

        /// <summary>
        /// 未解决
        /// </summary>
        [Description("未解决")]
        UnSolve = 3,

        /// <summary>
        /// 验证关闭
        /// </summary>
        [Description("验证关闭")]
        Closed,

        /// <summary>
        /// 挂起
        /// </summary>
        [Description("挂起")]
        HasHangUp,

        /// <summary>
        /// 暂存
        /// </summary>
        [Description("暂存")]
        HasTemporary,

        /// <summary>
        /// 待验证
        /// </summary>
        [Description("待验证")]
        HasRechecked


        //问题状态变更
        //已开启----->新建
        //已分派----->处理中
        //已处理----->待复核
        //已复核----->待验证
        //已关闭----->验证关闭
        //已挂起----->挂起
        //已暂存----->暂存
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

    /// <summary>
    /// 操作按钮枚举
    /// </summary>
    public enum EnumIssueButton
    {
        /// <summary>
        /// 复制
        /// </summary>
        [Description("复制")]
        Copy = 0,

        /// <summary>
        /// 编辑
        /// </summary>
        [Description("编辑")]
        Edit = 1,

        /// <summary>
        /// 详情
        /// </summary>
        [Description("详情")]
        Detail = 2,

        /// <summary>
        /// 分发
        /// </summary>
        [Description("分发")]
        Dispatch = 3,

        /// <summary>
        /// 转交
        /// </summary>
        [Description("转交")]
        ReDispatch = 4,

        /// <summary>
        /// 解决
        /// </summary>
        [Description("解决")]
        Execute = 5,

        /// <summary>
        /// 复核
        /// </summary>
        [Description("复核")]
        ReCheck = 6,

        /// <summary>
        /// 验证
        /// </summary>
        [Description("验证")]
        Validate = 7,

        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 8,

        /// <summary>
        /// 挂起
        /// </summary>
        [Description("挂起")]
        HangUp = 9,

        /// <summary>
        /// 重开启
        /// </summary>
        [Description("重开启")]
        ReOpen = 10,

        /// <summary>
        /// 催办
        /// </summary>
        [Description("催办")]
        Notice = 11,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 12,
    }
}