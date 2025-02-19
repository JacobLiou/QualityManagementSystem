﻿using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 问题来源
    /// </summary>
    public enum EnumIssueSource
    {
        [Description("客户反馈")]
        Customer = 0,

        [Description("工厂")]
        Factory = 1,

        [Description("测试发现")]
        Test = 2,

        [Description("外企厂")]
        ForeignEnterpriseFactory = 3,

        [Description("实验室")]
        Laboratory = 4,
    }
}