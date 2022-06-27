using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Entity.Common.Enum;
using Furion.Extras.Admin.NET.Service;
using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Application.System
{
    /// <summary>
    /// 产品输入参数
    /// </summary>
    public class SsuProductInput : PageInputBase
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public virtual string ProductType { get; set; }

        ///// <summary>
        ///// 产品线
        ///// </summary>
        //public virtual EnumProductLine? ProductLine { get; set; }

        ///// <summary>
        ///// 所属项目
        ///// </summary>
        //public virtual long ProjectId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual Furion.Extras.Admin.NET.Entity.Common.Enum.EnumProductStatus Status { get; set; }

        /// <summary>
        /// 产品分类
        /// </summary>
        public virtual Furion.Extras.Admin.NET.Entity.Common.Enum.EnumProductClassfication ClassificationId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }

        /// <summary>
        /// 产品负责人
        /// </summary>
        public virtual long DirectorId { get; set; }

        /// <summary>
        /// 关联人员列表
        /// </summary>
        public virtual IEnumerable<long> UserIdList { get; set; }
    }

    public class AddSsuProductInput
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public virtual string ProductType { get; set; }

        ///// <summary>
        ///// 产品线
        ///// </summary>
        //public virtual EnumProductLine? ProductLine { get; set; }

        ///// <summary>
        ///// 所属项目
        ///// </summary>
        //public virtual long ProjectId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual Furion.Extras.Admin.NET.Entity.Common.Enum.EnumProductStatus Status { get; set; }

        /// <summary>
        /// 产品分类
        /// </summary>
        public virtual Furion.Extras.Admin.NET.Entity.Common.Enum.EnumProductClassfication ClassificationId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public virtual int Sort { get; set; }

        /// <summary>
        /// 产品负责人
        /// </summary>
        [Required]
        public virtual long DirectorId { get; set; }

        /// <summary>
        /// 关联人员列表
        /// </summary>
        public virtual IEnumerable<long> UserIdList { get; set; }
    }

    public class DeleteSsuProductInput : BaseId
    {
    }

    public class UpdateSsuProductInput : SsuProductInput
    {
        /// <summary>
        /// Id主键
        /// </summary>
        [Required(ErrorMessage = "Id主键不能为空")]
        public long Id { get; set; }
    }

    public class QueryeSsuProductInput : BaseId
    {
    }

    /// <summary>
    /// 获取产品对应人员的输入参数
    /// </summary>
    public class SsuProductUserInput : PageInputBase
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [Required(ErrorMessage = "请输入产品ID")]
        public long productId { get; set; }
    }
}