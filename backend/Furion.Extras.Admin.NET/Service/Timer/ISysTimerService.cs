using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysTimerService
    {
        Task AddTimer(AddJobInput input);

        void AddTimerJob(AddJobInput input);

        Task DeleteTimer(DeleteJobInput input);

        Task<SysTimer> GetTimer([FromQuery] QueryJobInput input);

        Task<PageResult<JobOutput>> GetTimerPageList([FromQuery] JobPageInput input);

        void StartTimerJob(AddJobInput input);

        void StopTimerJob(StopJobInput input);

        Task UpdateTimber(UpdateJobInput input);

        void StartTimerJob();
    }
}