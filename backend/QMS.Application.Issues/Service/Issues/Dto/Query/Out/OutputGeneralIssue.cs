using Furion.Extras.Admin.NET.Service;
using QMS.Application.Issues.Helper;
using QMS.Core.Enum;

namespace QMS.Application.Issues.Service.Issue.Dto.Query
{
    public class OutputGeneralIssue : BaseId
    {
        public string Title { get; set; }

        public string ProjectName { get; set; }
        public long ProjectId { get; set; }

        public string ProductName { get; set; }
        public long ProductId { get; set; }

        public EnumModule Module { get; set; }
        public EnumConsequence Consequence { get; set; }
        public EnumIssueClassification IssueClassification { get; set; }
        public EnumIssueSource Source { get; set; }
        public EnumIssueStatus Status { get; set; }

        public long? Creator { get; set; }
        public string CreatorName { get; set; }

        public DateTime? CreateTime { get; set; }
        public DateTime? CloseTime { get; set; }

        public long? Discover { get; set; }
        public string DiscoverName { get; set; }
        public DateTime? DiscoverTime { get; set; }

        
        public long? Dispatcher { get; set; }
        public string DispatcherName { get; set; }
        public DateTime? DispatchTime { get; set; }
        
        public DateTime? ForecastSolveTime { get; set; }

        public long? CopyTo { get; set; }
        public string CopyToName { get; set; }

        public long? Executor { get; set; }
        public string ExecutorName { get; set; }
        public DateTime? SolveTime { get; set; }

        public long? Verifier { get; set; }
        public string VerifierName { get; set; }
        public string VerifierPlace { get; set; }
        public DateTime? ValidateTime { get; set; }

        public OutputGeneralIssue(QMS.Core.Entity.Issue model)
        {
            this.Id = model.Id;
            this.Title = model.Title;
            this.Module = model.Module;
            this.Consequence = model.Consequence;
            this.IssueClassification = model.IssueClassification;
            this.Source = model.Source;
            this.Status = model.Status;
            this.CreateTime = model.CreateTime;
            this.DiscoverTime = model.DiscoverTime;
            this.DispatchTime = model.DispatchTime;
            this.ForecastSolveTime = model.ForecastSolveTime;
            this.SolveTime = model.SolveTime;
            this.VerifierPlace = model.VerifierPlace;
            this.ValidateTime = model.ValidateTime;

            this.Creator = model.CreatorId;
            this.CreatorName = model.CreatorId.GetNameByEmpId();

            this.Dispatcher = model.Dispatcher;
            this.DispatcherName = model.Dispatcher.GetNameByEmpId();
            
            this.ProjectName = model.ProjectId.GetNameByProjectId();
            this.ProjectId = model.ProjectId;

            this.ProductName = model.ProductId.GetNameByProductId();
            this.ProductId = model.ProductId;

            this.Discover = model.Discover;
            this.DiscoverName = model.Discover.GetNameByEmpId();

            this.Executor = model.Executor;
            this.ExecutorName = model.Executor.GetNameByEmpId();

            this.Verifier = model.Verifier;
            this.VerifierName = model.Verifier == null ? this.CreatorName : model.Verifier.GetNameByEmpId();
            
            this.CloseTime = model.CloseTime;

            this.CopyTo = model.CC;
            this.CopyToName = model.CC.GetNameByEmpId();
        }
    }
}
