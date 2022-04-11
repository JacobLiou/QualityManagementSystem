using QMS.Application.Issues.Dto;
using QMS.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Mapster;
using QMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{
    public class SsuesService:ITransient
    {
        private readonly IRepository<SsuIssues, IssuesDbContextLocator> _ssuIssues;  // 问题表仓储

        public SsuesService(IRepository<SsuIssues, IssuesDbContextLocator> ssuIssues)
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
