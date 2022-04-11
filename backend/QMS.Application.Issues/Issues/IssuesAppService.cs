using QMS.Application.Issues.Dto;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{

    /// <summary>
    /// 问题管理
    /// </summary>
    [ApiDescriptionSettings("问题管理", Name = "Issues", Order = 100)]
    public class IssuesAppService: IDynamicApiController
    {
        private readonly SsuesService _ssuesService;
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssuesService"></param>
        public IssuesAppService(SsuesService ssuesService)
        {
            _ssuesService = ssuesService;
        }

        /// <summary>
        /// 获取问题描述
        /// </summary>
        /// <returns></returns>
        public IssuesOutput GetTest()
        {
            return _ssuesService.GetTest();
        }

        public IssuesOutput GetTest1()
        {
            return _ssuesService.GetTest();
        }
    }
}
