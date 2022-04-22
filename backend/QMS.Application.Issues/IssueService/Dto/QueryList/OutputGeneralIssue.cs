using Furion.Extras.Admin.NET.Service;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;

namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class OutputGeneralIssue : BaseId
    {
        public OutputGeneralIssue(SsuIssue model)
        {
            this.Consequence = model.Consequence.GetEnumDescription<EnumConsequence>();
            this.CreatorName = model.CreatorId.GetNameByEmpId();
            this.CreateTime = model.CreateTime.GetTimeString();
            this.ForecastSolveTime = model.ForecastSolveTime.GetDateString();
            this.Status = model.Status.GetEnumDescription<EnumIssueStatus>();
            this.Dispatcher = model.Dispatcher.GetNameByEmpId();
            this.Source = model.Source.GetEnumDescription<EnumIssueSource>();
            this.ProjectName = model.ProjectId.GetNameByProjectId();
            this.ProductName = model.ProductId.GetNameByProductId();
            this.Module = model.Module.GetEnumDescription<EnumModule>();
            this.IssueClassification = model.IssueClassification.GetEnumDescription<EnumIssueClassification>();

            this.Discover = model.Discover.GetNameByEmpId();
            this.DiscoverTime = model.DiscoverTime.GetDateString();
            this.Executor = model.Executor.GetNameByEmpId();
            this.SolveTime = model.SolveTime.GetDateString();
            this.CloseTime = model.CloseTime.GetTimeString();
            this.Verifier = model.Verifier.GetNameByEmpId();
            this.ValidateTime = model.ValidateTime.GetDateString();

            base.Id = model.Id;
        }


        public string Consequence { get; set; }
        public string Title { get; set; }
        public string CreatorName { get; set; }
        public string CreateTime { get; set; }
        public string ForecastSolveTime { get; set; }
        public string Status { get; set; }
        public string Dispatcher { get; set; }
        public string Source { get; set; }

        public string ProjectName { get; set; }

        public string ProductName { get; set; }

        public string Module { get; set; }

        public string IssueClassification { get; set; }

        public string Discover { get; set; }
        public string DiscoverTime { get; set; }
        public string Executor { get; set; }

        public string SolveTime { get; set; }

        public string CloseTime { get; set; }
        public string VerifierPlace { get; set; }
        public string Verifier { get; set; }

        public string ValidateTime { get; set; }
    }
}
