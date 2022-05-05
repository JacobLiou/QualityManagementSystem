using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.System.Service;

namespace QMS.Application.System
{
    public class SystemService : ISystemService, ITransient
    {
        private readonly ISqlRepository<MasterDbContextLocator> _sqlRepository;

        public SystemService(ISqlRepository<MasterDbContextLocator> sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }
        public string GetDescription()
        {
            return "让 .NET 开发更简单，更通用，更流行。";
        }

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public List<GroupUserOutput> GetUserGroup()
        {
            var list = _sqlRepository.SqlQuery<GroupUserOutput>(@"SELECT u.`Name` as UserName,u.Id AS UserID,g.GroupName,g.ID AS GroupId FROM `ssu_group`  g JOIN ssu_group_user gu on g.Id = gu.GroupId 
join sys_user  u on gu.EmployeeId = u.Id where u.Id > @id", new { id = 10 });
            return list;
        }

     /// <summary>
     /// 
     /// </summary>
     /// <returns></returns>
        public List<SsuGroupOutput> GetGroup()
        {
            var list = _sqlRepository.SqlQuery<SsuGroupOutput>("select * from ssu_group where id > @id", new { id = 10 });
            return list;
        }


    }
}