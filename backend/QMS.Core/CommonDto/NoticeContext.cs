using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core
{
    public class NoticeContext
    {
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型（字典 1通知 2公告）
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 发布人Id
        /// </summary>
        public long PublicUserId { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTimeOffset? PublicTime { get; set; }

        public List<string> NoticeUserIdList { get; set; }
        /// <summary>
        /// 问题页面URL
        /// </summary>
        public string PageUrl {get;set;}
    }
}
