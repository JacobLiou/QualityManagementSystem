using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QMS.Core.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{
    [Table("issue")]
    [Comment("问题记录")]
    public class Issue : DEntityTenant, IEntity<IssuesDbContextLocator>, IEntityTypeBuilder<Issue, IssuesDbContextLocator>
    {
        [Comment("问题简述")]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Comment("项目编号")]
        public long ProjectId { get; set; }

        //public SsuProject Project { get; set; }

        [Comment("产品编号")]
        public long? ProductId { get; set; }

        //public SsuProduct Product { get; set; }

        [Comment("问题模块")]
        public EnumModule Module { get; set; }

        [Comment("问题性质")]
        public EnumConsequence Consequence { get; set; }


        [Comment("问题分类")]
        public EnumIssueClassification IssueClassification { get; set; }


        [Comment("问题来源")]
        public EnumIssueSource? Source { get; set; }

        [Comment("问题状态")]
        public EnumIssueStatus Status { get; set; }


        [Comment("提出人")]
        public long CreatorId { get; set; }


        [Comment("提出日期")]
        public DateTime CreateTime { get; set; }

        public void SetCreate(bool isTemporary)
        {
            this.CreateTime = DateTime.Now;
            this.CreatorId = CurrentUserInfo.UserId;

            if (isTemporary)
            {
                this.Status = EnumIssueStatus.HasTemporary;
            }
            else
            {
                this.Status = EnumIssueStatus.Created;
            }

            //此时问题处于已开启状态，数据库先不记录验证人，等验证步骤之后记录
            //if (this.Verifier == null)
            //{
            //    this.Verifier = this.CreatorId;
            //}
            //问题新增时不记录分发人
            //this.CurrentAssignment = this.Dispatcher;
        }


        [Comment("关闭日期")]
        public DateTime? CloseTime { get; set; }

        public void SetClose()
        {
            this.CloseTime = DateTime.Now;

            this.Status = EnumIssueStatus.Closed;
        }

        [Comment("发现人")]
        public long? Discover { get; set; }

        [Comment("发现日期")]
        public DateTime? DiscoverTime { get; set; }

        [Comment("分发人")]
        public long? Dispatcher { get; set; }

        [Comment("分发日期")]
        public DateTime? DispatchTime { get; set; }

        public void DoDispatch()
        {
            this.DispatchTime = DateTime.Now;
            this.Dispatcher = CurrentUserInfo.UserId;

            this.Status = EnumIssueStatus.Dispatched;

            //this.CurrentAssignment = this.Executor;
        }

        public void DoReCheck(bool pass)
        {
            this.Status = pass ? EnumIssueStatus.HasRechecked : EnumIssueStatus.Dispatched;

            this.CurrentAssignment = pass ? this.CreatorId : this.Executor;
        }

        public void DoReOpen()
        {
            this.Status = EnumIssueStatus.Created;
        }

        [Comment("预计完成日期")]
        public DateTime? ForecastSolveTime { get; set; }

        //[Comment("被抄送人")]
        //public long? CC { get; set; }

        /// <summary>
        /// List<long>.ToJson()
        /// </summary>
        [Comment("被抄送人")]
        public string CCs { get; set; }


        [Comment("解决人")]
        public long? Executor { get; set; }

        [Comment("解决日期")]
        public DateTime? SolveTime { get; set; }

        public void DoSolve()
        {
            //解决日期应界面上的日期
            //this.SolveTime = DateTime.Now;
            this.Executor = CurrentUserInfo.UserId;

            this.Status = EnumIssueStatus.Solved;

            this.CurrentAssignment = this.Dispatcher;
        }

        [Comment("验证人")]
        public long? Verifier { get; set; }

        [Comment("验证地点")]
        [MaxLength(200)]
        public string VerifierPlace { get; set; }

        [Comment("验证日期")]
        public DateTime? ValidateTime { get; set; }

        public void DoVerify(bool pass)
        {
            this.Verifier = CurrentUserInfo.UserId;
            this.ValidateTime = DateTime.Now;

            this.Status = pass ? EnumIssueStatus.Closed : EnumIssueStatus.UnSolve;

            this.ValidationStatus = pass ? 2 : 1;

            this.CurrentAssignment = pass ? this.CreatorId : this.Dispatcher;

            if (pass)
            {
                this.CloseTime = DateTime.Now;
            }
        }

        [Comment("挂起人")]
        public long? HangupId { get; set; }

        public void SetHangup()
        {
            this.HangupId = CurrentUserInfo.UserId;

            this.Status = EnumIssueStatus.HasHangUp;
        }


        public void Configure(EntityTypeBuilder<Issue> entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            //entityBuilder.HasMany(i => i.SsuIssueOperations)
            //    .WithOne(i => i.Issue)
            //    .HasForeignKey(i => i.IssueId);

            entityBuilder.HasOne(i => i.SsuIssueDetail)
                .WithOne(i => i.Issue)
                .HasForeignKey<IssueDetail>(i => i.Id);
        }

        /// <summary>
        /// 0: 没有走到验证  1: 验证不通过 2: 验证通过
        /// </summary>
        [Comment("回归验证状态")]
        public int ValidationStatus { get; set; }

        /// <summary>
        /// 当前指派给
        /// </summary>
        [Comment("当前指派给")]
        public long? CurrentAssignment { get; set; }

        [NotMapped]
        public ICollection<IssueExtendAttribute> Attrs { get; set; }

        [NotMapped]
        public ICollection<IssueExtendAttributeValue> AttrValues { get; set; }

        [NotMapped]
        public ICollection<IssueOperation> SsuIssueOperations { get; set; }

        [NotMapped]
        public IssueDetail SsuIssueDetail { get; set; }
    }
}