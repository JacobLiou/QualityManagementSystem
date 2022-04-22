using Furion.DataValidation;
using Furion.Extras.Admin.NET.Service;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.Issues.IssueService.Dto.New
{
    public class InCommonNewWithOutId
    {
        public long ProjectId { get; set; }
        public long ProductId { get; set; }
        public int Module { get; set; }
        public int IssueClassification { get; set; }
        public long Dispatcher { get; set; }
        public int Consequence { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Source { get; set; }
        public long Discover { get; set; }
        public string DiscoverTime { get; set; }
        public long CC { get; set; }
        /// <summary>
        /// Json Array
        /// </summary>
        public string Attachments { get; set; }
    }


    public class InCommonNew : InCommonNewWithOutId
    {
        [Required(ErrorMessage = "Id不能为空")]
        [DataValidation(ValidationTypes.Numeric)]
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long ProductId { get; set; }
        public int Module { get; set; }
        public int IssueClassification { get; set; }
        public long Dispatcher { get; set; }
        public int Consequence { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Source { get; set; }
        public long Discover { get; set; }
        public string DiscoverTime { get; set; }
        public long CC { get; set; }
        /// <summary>
        /// Json Array
        /// </summary>
        public string Attachments { get; set; }
    }
}
