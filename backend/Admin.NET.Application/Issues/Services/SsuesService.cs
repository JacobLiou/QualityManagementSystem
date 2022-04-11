using Admin.NET.Application.Issues.Dto;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Issues
{
    public class SsuesService:ITransient
    {
        private readonly IRepository<SsuIssues, MultiTenantDbContextLocator> _ssuIssues;  // 菜单表仓储

        public SsuesService(IRepository<SsuIssues, MultiTenantDbContextLocator> ssuIssues)
        {
            _ssuIssues = ssuIssues;
        }
        public IssuesOutput GetTest()
        {
            SsuIssues issues = _ssuIssues.First();


            return issues.Adapt<IssuesOutput>();
        }
    }
}
