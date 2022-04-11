using QMS.Core.Entity;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues.Dto
{
    public class Mapper : IRegister
    {

        public void Register(TypeAdapterConfig config)
        {
            config.ForType<SsuIssues, IssuesOutput>()
                   .Map(dest => dest.Name, src => src.Title);
        }
    }

}
