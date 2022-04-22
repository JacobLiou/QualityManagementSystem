﻿using System.ComponentModel;

namespace QMS.Core.Enum
{
    public enum EnumVeneer
    {
        [Description("整机")]
        Whole = 0,

        [Description("功率板")]
        PowerBoard,

        [Description("控制板")]
        ControlBoard,

        [Description("输出板")]
        OutputBoard,

        [Description("升压驱动板")]
        BoostDriveBoard,

        [Description("EMI板")]
        EMIBoard,
    }
}
