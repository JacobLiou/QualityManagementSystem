using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System
{
    /// <summary>
    /// 通用返回结果类
    /// </summary>
    public class CommonOutput
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
}