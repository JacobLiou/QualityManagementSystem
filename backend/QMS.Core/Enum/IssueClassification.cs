using System.ComponentModel;

namespace QMS.Core.Enum
{
    public enum EnumIssueClassification
    {
        [Description("设计问题-硬件")]
        Hardware = 0,
        [Description("设计问题-产品软件")]
        Software = 1,
        [Description("设计问题-生产测试")]
        Test = 2,
        [Description("设计问题-结构")]
        Structure = 3,
        [Description("设计问题-工艺")]
        DesignTechnology = 4,
        [Description("设计问题-BOM")]
        BOM = 5,
        [Description("设计问题-器件")]
        Device = 6,
        [Description("设计问题-包装")]
        Pack = 7,
        [Description("设计问题-治具")]
        Fixture = 8,
        [Description("制程问题-SMT")]
        SMT = 9,
        [Description("制程问题-DIP")]
        DIP = 10,
        [Description("制程问题-工艺")]
        ProcessTechnology = 11,
        [Description("制程问题-设备")]
        ProcessEquipment = 12,
        [Description("制程问题-装配")]
        ProcessAssembly = 13,
        [Description("物料问题")]
        Material = 14,
        [Description("其它问题")]
        Other = 15,
    }
}