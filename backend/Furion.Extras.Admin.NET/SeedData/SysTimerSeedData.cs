﻿using Furion.DatabaseAccessor;
using Furion.TaskScheduler;
using Microsoft.EntityFrameworkCore;

namespace Furion.Extras.Admin.NET
{
    /// <summary>
    /// 系统任务调度表种子数据
    /// </summary>
    public class SysTimerSeedData : IEntitySeedData<SysTimer>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysTimer> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysTimer
                {
                    Id = 142307070910556,
                    JobName = "百度api",
                    DoOnce = false,
                    StartNow = false,
                    Interval = 5,
                    TimerType = SpareTimeTypes.Interval,
                    ExecuteType = SpareTimeExecuteTypes.Serial,
                    RequestUrl = "https://www.baidu.com",
                    RequestType = RequestTypeEnum.Post,
                    IsDeleted = false,
                    Remark = "接口API"
                }
            };
        }
    }
}