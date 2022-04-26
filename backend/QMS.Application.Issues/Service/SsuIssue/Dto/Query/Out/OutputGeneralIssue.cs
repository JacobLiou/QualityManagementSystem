using Furion.Extras.Admin.NET.Service;
using QMS.Core.Enum;

namespace QMS.Application.Issues.IssueService.Dto.QueryList
{
    public class OutputGeneralIssue : BaseId
    {
        //public OutputGeneralIssue(SsuIssue model)
        //{
        //    this.Consequence = model.Consequence.GetEnumDescription<EnumConsequence>();
        //    this.CreatorName = model.CreatorId.GetNameByEmpId();
        //    this.CreateTime = model.CreateTime.GetTimeString();
        //    this.ForecastSolveTime = model.ForecastSolveTime.GetDateString();
        //    this.Status = model.Status.GetEnumDescription<EnumIssueStatus>();
        //    this.Dispatcher = model.Dispatcher.GetNameByEmpId();
        //    this.Source = model.Source.GetEnumDescription<EnumIssueSource>();
        //    this.ProjectName = model.ProjectId.GetNameByProjectId();
        //    this.ProductName = model.ProductId.GetNameByProductId();
        //    this.Module = model.Module.GetEnumDescription<EnumModule>();
        //    this.IssueClassification = model.IssueClassification.GetEnumDescription<EnumIssueClassification>();

        //    this.Discover = model.Discover.GetNameByEmpId();
        //    this.DiscoverTime = model.DiscoverTime?.GetDateString();
        //    this.Executor = model.Executor.GetNameByEmpId();
        //    this.SolveTime = model.SolveTime?.GetDateString();
        //    this.CloseTime = model.CloseTime?.GetTimeString();
        //    this.Verifier = model.Verifier == 0 ? this.CreatorName : model.Verifier.GetNameByEmpId();
        //    this.ValidateTime = model.ValidateTime?.GetDateString();

        //    base.Id = model.Id;
        //    this.Title = model.Title;
        //    this.CloseTime = model.CloseTime.GetDateString();
        //    this.DispatchTime = model.DispatchTime?.GetTimeString();
        //    this.CC = model.CC.GetNameByEmpId();
        //    this.VerifierPlace = model.VerifierPlace;
        //}

        public string Title { get; set; }
        public string ProjectName { get; set; }
        public string ProductName { get; set; }
        public EnumModule Module { get; set; }
        public EnumConsequence Consequence { get; set; }
        public EnumIssueClassification IssueClassification { get; set; }
        public EnumIssueSource Source { get; set; }
        public EnumIssueStatus Status { get; set; }
        public long CreatorName { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public long Discover { get; set; }
        public DateTime? DiscoverTime { get; set; }
        public long Dispatcher { get; set; }
        public DateTime? DispatchTime { get; set; }
        public DateTime? ForecastSolveTime { get; set; }
        public long CC { get; set; }
        public long Executor { get; set; }
        public DateTime? SolveTime { get; set; }
        public long Verifier { get; set; }
        public string VerifierPlace { get; set; }
        public DateTime? ValidateTime { get; set; }
    }
}
