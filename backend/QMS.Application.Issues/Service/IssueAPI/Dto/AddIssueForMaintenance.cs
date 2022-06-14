using QMS.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues.Service.IssueAPI
{
    /// <summary>
    /// 运维问题添加模板
    /// </summary>
    public class AddIssueForMaintenance
    {
        /// <summary>
        /// 问题标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 附件ID
        /// </summary>
        public List<long> Attachments { get; set; }
        /// <summary>
        /// 客户信息
        /// </summary>
        public CustomerInfo Customer { get; set; }
        /// <summary>
        /// 设备信息
        /// </summary>
        public DeviceInfo Device { get; set; }

        /// <summary>
        /// 问题后果/问题性质
        /// </summary>
        public EnumConsequence Consequence { get; set; }
        /// <summary>
        /// 发现时间
        /// </summary>
        public DateTime? DiscoverTime { get; set; }
        /// <summary>
        /// 发现人名称
        /// </summary>
        public string DiscoverName { get; set; }
        /// <summary>
        /// 发现人联系方式
        /// </summary>
        public string DiscoverContact { get; set; }
        /// <summary>
        /// 期望完成时间
        /// </summary>
        public string ExpectedTime { get; set; }
        /// <summary>
        /// 指派人名称
        /// </summary>
        public string AssignmentName { get; set; }

    }
    /// <summary>
    /// 产品信息
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceModel { get; set; }
        /// <summary>
        /// 硬件版本
        /// </summary>
        public string HardwareVersion { get; set; }
        /// <summary>
        /// 软件版本
        /// </summary>
        public string SoftwareVersion { get; set; }
    }

    /// <summary>
    /// 客户信息
    /// </summary>
    public class CustomerInfo
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 国家/地区
        /// </summary>
        public string CountryRegion { get; set; }
    }
}
