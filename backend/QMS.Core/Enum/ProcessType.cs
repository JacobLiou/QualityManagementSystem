using System.ComponentModel;

namespace QMS.Core.Enum
{
    /// <summary>
    /// 售后问题-处理分类
    /// </summary>
    public enum EnumProcessType
    {
        [Description("退机")]
        ReturnMachine = 0,

        [Description("返修")]
        Repair,

        [Description("换新机")]
        ExchangeNew,

        [Description("换旧机")]
        ExchangeOld,
    }
}
