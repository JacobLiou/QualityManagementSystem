using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Issues
{
    public class IssuesInput
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        

        /// <summary>
        /// 问题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string Description { get; set; }


    }
}

