using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET.Entity.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System.Service
{
    [AllowAnonymous]
    public class MonitorAppService : IDynamicApiController, ITransient
    {
        private readonly IRepository<MonitorUser> _monitorUser; // 用户表
        private readonly IRepository<MonitorCode> _monitorCode;  // 注册码
        public MonitorAppService(IRepository<MonitorUser> monitorUser
            , IRepository<MonitorCode> monitorCode)
        {
            _monitorUser = monitorUser;
            _monitorCode = monitorCode;
        }
        [HttpGet("system/monitorApp/register")]
        public string RegisterMonitor(string code, string guid, string name)
        {

            var data = _monitorCode.FirstOrDefault(x => x.Code == code);
            if (data == null)
            {
                return null;
            }
            MonitorUser user = new MonitorUser();
            user.Name = name;
            user.Code = code;
            user.Machine = guid;
            user.CreatedTime = DateTime.Now;
            var insertUser = _monitorUser.Insert(user);
            if (insertUser == null)
            {
                return null;
            }
            string check = guid + "|" + data.Role;

            return check;
        }

    }
}
