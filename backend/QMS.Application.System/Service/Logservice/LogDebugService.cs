using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using QMS.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System.Service.Log
{
    public class LogDebugService : IDynamicApiController, ITransient, ILogDebugService
    {
        private readonly IRepository<SysLogDebug> _sysLogDebug; 
        public LogDebugService(IRepository<SysLogDebug> sysLogDebug)
        {
            _sysLogDebug = sysLogDebug;
        }
        /// <summary>
        /// 填写日志记录
        /// </summary>
        /// <param name="sysLogDebug"></param>
        /// <returns></returns>
        public async Task AddLogDebug(SysLogDebug sysLogDebug)
        {
            await _sysLogDebug.InsertAsync(sysLogDebug);
        }
    }
}
