using QMS.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System.Service.Log
{
    public interface ILogDebugService
    {
        public Task AddLogDebug(SysLogDebug sysLogDebug);
    }
}
