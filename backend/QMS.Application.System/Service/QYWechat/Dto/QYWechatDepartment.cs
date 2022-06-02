using System.Text.Json.Serialization;

namespace QMS.Application.System
{
    /// <summary>
    /// 企业微信部门
    /// </summary>
    public class QYWechatDepartmentList
    {
        /// <summary>
        ///返回码
        /// </summary>
        [JsonPropertyName("errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 返回码描述
        /// </summary>
        [JsonPropertyName("errmsg")]
        public string ErrMsg { get; set; }

        /// <summary>
        ///部门列表
        /// </summary>
        [JsonPropertyName("department")]
        public List<QYWechatDepartmentDetail> DepartmentDetailList { get; set; }
    }

    /// <summary>
    /// 企业微信部门详细
    /// </summary>
    public class QYWechatDepartmentDetail
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [JsonPropertyName("name_en")]
        public string Code { get; set; }

        /// <summary>
        ///部门负责人
        /// </summary>
        [JsonPropertyName("department_leader")]
        public IEnumerable<string> Department_Leader { get; set; }

        /// <summary>
        /// 父部门ID
        /// </summary>
        [JsonPropertyName("parentid")]
        public int Pid { get; set; }

        /// <summary>
        /// 父部门列表ID
        /// </summary>
        public string Pids { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonPropertyName("order")]
        public int Sort { get; set; }

        /// <summary>
        /// 状态（启用）
        /// </summary>
        public int status { get; set; } = 0;

        /// <summary>
        /// 租户ID
        /// </summary>
        public long TenantId { get; set; }
    }

    /// <summary>
    /// 企业微信部门用户
    /// </summary>
    public class QYWechatDepartmentUserList
    {
        /// <summary>
        ///返回码
        /// </summary>
        [JsonPropertyName("errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 返回码描述
        /// </summary>
        [JsonPropertyName("errmsg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// 用户列表
        /// </summary>
        [JsonPropertyName("userlist")]
        public IEnumerable<QYUserInfoModel> UserList { get; set; }
    }
}