﻿namespace QMS.Application.Issues.Service.SsuIssue.Dto.Update
{
    public class InSolve : SolveCommon
    {
        /// <summary>
        /// 原因分析
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 解决措施
        /// </summary>
        public string Measures { get; set; }

        /// <summary>
        /// 解决版本
        /// </summary>
        public string SolveVersion { get; set; }
    }
}
