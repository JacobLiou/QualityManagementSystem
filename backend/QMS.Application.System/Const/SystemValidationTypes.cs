using Furion.DataValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    /// <summary>
    /// 数据验证特性
    /// </summary>
    [ValidationType]
    public enum SystemValidationTypes
    {
        /// <summary>
        /// 强密码类型
        /// </summary>
        [ValidationItemMetadata(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$", "必须须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-10之间")]
        StrongPassword,

        /// <summary>
        /// 非负整数
        /// </summary>
        [ValidationItemMetadata(@"^[1-9]\d*|0$", "必须大于等于0的整数")]
        NumberUnNegativeNumber
    }
}