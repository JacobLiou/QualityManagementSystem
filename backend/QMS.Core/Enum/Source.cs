using System.ComponentModel;

namespace QMS.Core.Enum
{
    public enum EnumIssueSource
    {
        [Description("客户反馈")]
        Customer = 0,

        [Description("工厂")]
        Factory = 1,

        [Description("测试发现")]
        Test = 2,
    }
}
