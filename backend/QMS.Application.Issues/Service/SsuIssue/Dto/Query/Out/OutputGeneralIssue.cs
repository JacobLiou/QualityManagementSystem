using Furion.Extras.Admin.NET.Service;
using QMS.Application.Issues.Helper;
using QMS.Core.Entity;
using QMS.Core.Enum;

namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class OutputGeneralIssue : BaseId
    {
        public string Title { get; set; }
        public string ProjectName { get; set; }
        public string ProductName { get; set; }
        public EnumModule Module { get; set; }
        public EnumConsequence Consequence { get; set; }
        public EnumIssueClassification IssueClassification { get; set; }
        public EnumIssueSource Source { get; set; }
        public EnumIssueStatus Status { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public string Discover { get; set; }
        public DateTime? DiscoverTime { get; set; }
        public string Dispatcher { get; set; }
        public DateTime? DispatchTime { get; set; }
        public DateTime? ForecastSolveTime { get; set; }
        public string CopyTo { get; set; }
        public string Executor { get; set; }
        public DateTime? SolveTime { get; set; }
        public string Verifier { get; set; }
        public string VerifierPlace { get; set; }
        public DateTime? ValidateTime { get; set; }

        public OutputGeneralIssue(SsuIssue model)
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

            this.Creator = model.CreatorId.GetNameByEmpId();
            this.Dispatcher = model.Dispatcher.GetNameByEmpId();
            this.ProjectName = model.ProjectId.GetNameByProjectId();
            this.ProductName = model.ProductId.GetNameByProductId();
            this.Discover = model.Discover.GetNameByEmpId();
            this.Executor = model.Executor.GetNameByEmpId();
            this.Verifier = model.Verifier == null ? this.Creator : model.Verifier.GetNameByEmpId();
            
            this.CloseTime = model.CloseTime;
            this.CopyTo = model.CC.GetNameByEmpId();
        }
    }
}
