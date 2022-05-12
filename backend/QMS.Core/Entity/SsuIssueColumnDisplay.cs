﻿using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{
    [Table("ssu_issue_column_display")]
    [Comment("问题列表显示列明记录")]
    public class SsuIssueColumnDisplay //: IEntity<IssuesDbContextLocator>
    {

        [Comment("用户编号")]
        public long UserId { get; set; }

        /// <summary>
        /// [{title:“标题”}，{projectName:"项目名"}]
        /// </summary>
        [Comment("列名集合")]
        [MaxLength(50)]
        public string Columns { get; set; }
    }
}
