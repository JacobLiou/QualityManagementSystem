using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues.Service.IssueAPI
{
    /// <summary>
    /// 问题服务API
    /// </summary>
    [ApiDescriptionSettings("问题管理服务", Name = "IssueAPI", Order = 100)]
    public class IssueAPIService : IDynamicApiController, IScoped
    {
        /// <summary>
        /// 添加一个新问题
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        [HttpPost("issue/issueAPI/addIssue")]
        public long AddIssue(AddIssueForMaintenance issue)
        {
            return 100000;
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        [HttpGet("issue/issueAPI/queryStatus")]
        public async Task<IssueStatusDto> QueryStatus(string issueId)
        {
            return new IssueStatusDto() ;
        }

        /// <summary>
        /// 更新验证情况
        /// </summary>
        /// <param name="validateDto"></param>
        /// <returns></returns>
        [HttpPost("issue/issueAPI/updateValidate")]
        public bool UpdateValidate(UpdateValidateDto validateDto)
        {
            return true;
        }
    }
}
