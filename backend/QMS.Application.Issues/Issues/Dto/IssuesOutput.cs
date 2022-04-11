using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues.Dto
{
    public class IssuesOutput
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }



        /// <summary>
        /// 问题标题
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string Description { get; set; }

    }
}
