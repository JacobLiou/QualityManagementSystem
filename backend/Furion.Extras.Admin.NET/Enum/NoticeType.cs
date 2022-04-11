using System.ComponentModel;

namespace Furion.Extras.Admin.NET
{
    public enum NoticeType
    {
        /// <summary>
        /// 通知
        /// </summary>
        [Description("通知")]
        NOTICE = 1,

        /// <summary>
        /// 公告
        /// </summary>
        [Description("公告")]
        ANNOUNCEMENT = 2,
    }
}