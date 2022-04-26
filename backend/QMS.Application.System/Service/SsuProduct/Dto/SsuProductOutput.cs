using System;

namespace QMS.Application.System
{
    /// <summary>
    /// 产品输出参数
    /// </summary>
    public class SsuProductOutput
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductType { get; set; }
        
        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }
        
        /// <summary>
        /// 所属项目
        /// </summary>
        public long ProjectId { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public Furion.Extras.Admin.NET.Entity.Common.Enum.EnumProductStatus Status { get; set; }
        
        /// <summary>
        /// 产品分类
        /// </summary>
        public Furion.Extras.Admin.NET.Entity.Common.Enum.EnumProductClassfication ClassificationId { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 产品负责人
        /// </summary>
        public long DirectorId { get; set; }
        
        /// <summary>
        /// Id主键
        /// </summary>
        public long Id { get; set; }
        
    }
}
