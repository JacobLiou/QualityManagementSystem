using Furion;
using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.EventBus;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Furion.RemoteRequest.Extensions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace QMS.Application.System
{
    /// <summary>
    /// 企业微信扫码登录
    /// </summary>
    public class QYWeChatOAuth : IQYWeChatOAuth, ITransient
    {
        private readonly ICacheService _cache;
        private readonly IRepository<SysUser> _sysUserRep;  // 用户表仓储
        private readonly IRepository<SysOauthUser> _sysOauthUserRep;  // oauth用户表仓储
        private readonly IRepository<SysEmp> _sysEmpRep;  // 职员表仓储
        private readonly IRepository<SysEmpPos> _sysEmpPosRep;  // 员工职位表仓储
        private readonly IRepository<SysOrg> _sysOrgRep;  // 部门表仓储
        private readonly ILoginVerify _login;
        private readonly IRepository<SysUserRole> _sysUserRole;
        private readonly IConfiguration _configuration;

        ////企业微信重定向至登录界面
        //private readonly string RedirectUrl = "https://open.work.weixin.qq.com/wwopen/sso/qrConnect";

        ////企业微信获取access_token信息
        //private readonly string AccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";

        ////获取扫码用户ID
        //private readonly string UserIdUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo";

        ////获取扫码用户信息
        //private readonly string UserInfoUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/get";

        ////企业微信发送信息URL
        //private readonly string SendMessageUrl = " https://qyapi.weixin.qq.com/cgi-bin/message/send";

        ////网页授权链接
        //private readonly string WebOAuthUrl = "https://open.weixin.qq.com/connect/oauth2/authorize";

        ////获取部门列表
        //private readonly string DepartmentListUrl = "https://qyapi.weixin.qq.com/cgi-bin/department/list";

        ////获取部门下的用户列表
        //private readonly string DepartmentUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/list";

        //private readonly string Appid = "ww44dc5ededfe4a954";
        //private readonly string Corpsecret = "4-WoTdHkSNnUbnpxhI3PT1pAZTRmerr9AtsMV-HweDY";
        //private readonly string Agentid = "1000017";
        //private readonly string Status = "web_login@gyoss9";
        //private readonly long DefaultOrgId = 142307070910547;    //默认组织机构ID
        //private readonly string DefaultOrgName = "首航新能源";   //默认组织机构名称
        //private readonly long Role = 142307070910557;            //默认角色
        //private readonly long TenantId = 142307070918781;        //默认租户ID
        //private readonly long SysPos = 142307070910554;          //默认员工职位ID

        ////private readonly string LoginUrl = "http%3A%2F%2Fqms.sofarsolar.com:8001";
        //private readonly string LoginUrl = "http://qms.sofarsolar.com:8001/system/qyWechat/loginAndRegister";

        //private readonly int CacheHour = 2;

        public QYWeChatOAuth(ICacheService cache, IRepository<SysUser> user, IRepository<SysOauthUser> oauthUser, IRepository<SysEmp> emp,
            ILoginVerify loginVerify, IRepository<SysUserRole> sysUserRole, IRepository<SysEmpPos> sysEmpPosRep, IRepository<SysOrg> sysOrgRep,
            IConfiguration configuration)
        {
            _cache = cache;
            _sysUserRep = user;
            _sysOauthUserRep = oauthUser;
            _sysEmpRep = emp;
            _login = loginVerify;
            _sysUserRole = sysUserRole;
            _sysEmpPosRep = sysEmpPosRep;
            _sysOrgRep = sysOrgRep;
            _configuration = configuration;
        }

        /// <summary>
        /// 发起授权
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizeUrl()
        {
            var param = new Dictionary<string, string>()
            {
                ["appid"] = _configuration["QYWechatConfiguration:Appid"],
                ["agentid"] = _configuration["QYWechatConfiguration:Agentid"],
                ["redirect_uri"] = _configuration["QYWechatConfiguration:LoginUrl"],
                ["state"] = _configuration["QYWechatConfiguration:Status"]
            };
            return $"{_configuration["QYWechatConfiguration:RedirectUrl"]}?{param.ToQueryString()}#wechat_redirect";
        }

        /// <summary>
        /// 获取企业微信Token
        /// </summary>
        /// <returns></returns>
        public async Task<QYTokenModel> GetAccessTokenAsync()
        {
            QYTokenModel accessTokenModel = new QYTokenModel();
            var result = _cache.GetCache<string>(_configuration["QYWechatConfiguration:Appid"]).Result;
            if (!string.IsNullOrEmpty(result))
            {
                accessTokenModel.ErrMsg = "ok";
                accessTokenModel.ErrorCode = 0;
                accessTokenModel.AccessToken = result;
                return accessTokenModel;
            }
            var param = new Dictionary<string, string>()
            {
                ["corpid"] = _configuration["QYWechatConfiguration:Appid"],
                ["corpsecret"] = _configuration["QYWechatConfiguration:Corpsecret"]
            };
            accessTokenModel = await $"{_configuration["QYWechatConfiguration:AccessTokenUrl"]}?{param.ToQueryString()}".GetAsAsync<QYTokenModel>();
            if (accessTokenModel.HasError())
                throw Oops.Oh($"{accessTokenModel.ErrMsg}");
            await _cache.SetCacheByHours(_configuration["QYWechatConfiguration:Appid"], accessTokenModel.AccessToken, Convert.ToInt32(_configuration["QYWechatConfiguration:CacheHour"]));
            return accessTokenModel;
        }

        /// <summary>
        /// 获取微信用户ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<QYUserIdModel> GetQYUserIdAsync(string accessToken, string code)
        {
            var param = new Dictionary<string, string>()
            {
                ["access_token"] = accessToken,
                ["code"] = code
            };
            var userIdModel = await $"{_configuration["QYWechatConfiguration:UserIdUrl"]}?{param.ToQueryString()}".GetAsAsync<QYUserIdModel>();
            if (userIdModel.HasError())
                throw Oops.Oh($"{userIdModel.ErrMsg}");
            return userIdModel;
        }

        /// <summary>
        /// 获取企业微信用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<QYUserInfoModel> GetQYUserInfoAsync(string accessToken, string userId)
        {
            var paramUserInfo = new Dictionary<string, string>()
            {
                ["access_token"] = accessToken,
                ["userid"] = userId
            };
            var userInfoModel = await $"{_configuration["QYWechatConfiguration:UserInfoUrl"]}?{paramUserInfo.ToQueryString()}".GetAsAsync<QYUserInfoModel>();
            if (userInfoModel.HasError())
                throw Oops.Oh($"{userInfoModel.ErrMsg}");
            return userInfoModel;
        }

        /// <summary>
        /// 企业微信注册
        /// </summary>
        /// <param name="qYUserInfo">企业微信用户详细信息</param>
        /// <param name="sysUser">用户信息</param>
        /// <returns></returns>
        public SysUser QYWechatRegister(QYUserInfoModel qYUserInfo, SysUser sysUser)
        {
            //新增用户表
            if (sysUser == null)
            {
                var user = qYUserInfo.Adapt<SysUser>();
                user.Password = MD5Encryption.Encrypt("123456");
                user.Status = 0;
                user.TenantId = Convert.ToInt64(_configuration["TenantId"]);
                var newUser = _sysUserRep.InsertNow(user);
                sysUser = newUser.Entity;
            }
            //如果该用户已经在oauthUser表中存在对应的记录，则证明该用户已经绑定了企业微信，否则则新增
            var oauthUser = _sysOauthUserRep.DetachedEntities.Where(u => u.OpenId.Equals(sysUser.Id)).FirstOrDefault();
            if (oauthUser == null)
            {
                //新增oauthUser表
                var NewOauthUser = qYUserInfo.Adapt<SysOauthUser>();
                NewOauthUser.OpenId = sysUser.Id.ToString();
                NewOauthUser.Uuid = sysUser.Account;
                _sysOauthUserRep.InsertNow(NewOauthUser);
            }
            //如果职员表上不存在对应的记录则新增
            var emp = _sysEmpRep.DetachedEntities.Where(u => u.Id.Equals(sysUser.Id)).FirstOrDefault();
            if (emp == null)
            {
                //新增职员表
                var NewEmp = qYUserInfo.Adapt<SysEmp>();
                NewEmp.Id = sysUser.Id;
                //NewEmp.OrgId = DefaultOrgId;        //深圳首航默认ID
                //NewEmp.OrgName = DefaultOrgName;
                _sysEmpRep.InsertNow(NewEmp);
            }
            //如果员工职位表不存在对应的记录则新增
            var empPos = _sysEmpPosRep.DetachedEntities.Where(u => u.SysEmpId.Equals(sysUser.Id)).FirstOrDefault();
            if (empPos == null)
            {
                //新增员工职位表
                var newEmpPos = new SysEmpPos();
                newEmpPos.SysEmpId = sysUser.Id;
                newEmpPos.SysPosId = Convert.ToInt64(_configuration["SysPos"]);
                _sysEmpPosRep.InsertNow(newEmpPos);
            }

            //如果角色表上不存在对应的记录则新增
            var userRole = _sysUserRole.DetachedEntities.Where(u => u.SysUserId.Equals(sysUser.Id)).FirstOrDefault();
            if (userRole == null)
            {
                //新增用户对应的角色记录
                var NewUserRole = new SysUserRole();
                NewUserRole.SysUserId = sysUser.Id;
                NewUserRole.SysRoleId = Convert.ToInt64(_configuration["Role"]);   //默认角色ID
                _sysUserRole.InsertNow(NewUserRole);
            }
            return sysUser;
        }

        /// <summary>
        /// 企业微信登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string QYWechatLogin(SysUser user)
        {
            return _login.Login(user);
        }

        /// <summary>
        /// 发送企业微信消息
        /// </summary>
        /// <param name="touser">接收消息用户UserID列表</param>
        /// <param name="toparty">接收消息部门</param>
        /// <param name="totag">消息标签</param>
        /// <param name="title">标题</param>
        /// <param name="description">内容描述</param>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public async Task<string> QYWechatSendMessage(IEnumerable<string> touser, string toparty, string totag, string title, string description, string url)
        {
            //将用户ID转换成企业微信ID
            var tourIds = _sysOauthUserRep.DetachedEntities.Where(u => touser.Contains(u.OpenId)).AsQueryable().Select(u => u.Uuid).ToList();
            if (tourIds == null || tourIds.Count == 0)
            {
                throw Oops.Oh($"该用户不存在对应的企业微信ID");
            }
            string tousers = string.Join("|", tourIds);
            return await QYWechatSendMessage(tousers, toparty, totag, title, description, url);
        }

        /// <summary>
        /// 企业微信发送文字卡片消息
        /// </summary>
        /// <param name="touser">企业微信ID，多个ID通过 | 分割</param>
        /// <param name="toparty">接收消息部门</param>
        /// <param name="totag">消息标签</param>
        /// <param name="title">标题</param>
        /// <param name="description">内容描述</param>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public async Task<string> QYWechatSendMessage(string touser, string toparty, string totag, string title, string description, string url)
        {
            var token = GetAccessTokenAsync().Result.AccessToken;
            QYWechatMessage message = new QYWechatMessage();
            //构建从企业微信卡片消息跳转到平台的URL
            var paramUrl = new Dictionary<string, string>()
            {
                ["appid"] = _configuration["QYWechatConfiguration:Appid"],
                ["redirect_uri"] = url,
                ["response_type"] = "code",
                ["scope"] = "snsapi_base",
                ["state"] = "FromQYWechat"
            };
            url = $"{_configuration["QYWechatConfiguration:WebOAuthUrl"]}?{paramUrl.ToQueryString()}#wechat_redirect";

            message.Touser = touser;
            message.Toparty = toparty;
            message.Totag = totag;
            message.Msgtype = "textcard";
            message.Textcard = new Dictionary<string, string>()
            {
                ["title"] = title,
                ["description"] = description,
                ["url"] = url,
                ["btntxt"] = "更多"
            };
            message.Agentid = Convert.ToInt32(_configuration["QYWechatConfiguration:Agentid"]);
            var messResule = await $"{_configuration["QYWechatConfiguration:SendMessageUrl"]}?access_token={token}".SetBody(JSON.Serialize(message)).PostAsAsync<QYWechatResult>();
            if (messResule.Errcode != 0)
                throw Oops.Oh($"{messResule.Errmsg}");
            return $"发送信息成功,消息ID为{messResule.Msgid}";
        }

        /// <summary>
        /// 获取全部部门并新增
        /// </summary>
        /// <returns></returns>
        public async Task GetAllDepartment(string AccessToken)
        {
            //获取部门列表
            var DepartmentList = await $"{_configuration["QYWechatConfiguration:DepartmentListUrl"]}?access_token={AccessToken}".PostAsAsync<QYWechatDepartmentList>();

            //筛选不存在系统中的部门列表
            var newDepartLIst = DepartmentList.DepartmentDetailList.Where(u => !_sysOrgRep.DetachedEntities.IgnoreQueryFilters().Select(t => t.Id).Contains(u.Id));

            //构造父部门链
            SetPidsChain(newDepartLIst.ToList(), new List<long>() { 1 }, new List<int>() { 0 });

            //公司企业微信上没有对应的英文名称可以作为code使用，此处采用id+order拼接的方式来作为code值
            //设置默认租户ID
            newDepartLIst.ToList().ForEach(u => { u.Code = u.Id.ToString() + u.Sort; u.TenantId = Convert.ToInt64(_configuration["TenantId"]); });

            //按照部门列表新增部门记录
            var departList = newDepartLIst.AsQueryable().ProjectToType<SysOrg>().ToList();
            await _sysOrgRep.InsertNowAsync(departList);
        }

        /// <summary>
        /// 递归构造部门的父部门链
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pids"></param>
        /// <param name="idChains"></param>
        /// <returns></returns>
        private bool SetPidsChain(List<QYWechatDepartmentDetail> list, List<long> pids, List<int> idChains)
        {
            foreach (int id in pids)
            {
                var model = list.Where(u => u.Id.Equals(id)).FirstOrDefault();
                if (model != null)
                {
                    string parentIds = string.Join("", idChains.Select(u => "[" + u + "],"));
                    model.Pids = parentIds;
                }

                var newPids = list.Where(u => u.Pid == id).Select(u => u.Id).ToList();
                if (newPids != null && newPids.Count > 0)
                {
                    idChains.Add(id);
                    SetPidsChain(list, newPids, idChains);
                    idChains.Remove(id);
                }
            }
            return true;
        }


        /// <summary>
        /// 获取全部部门用户并新增
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        public async Task GetAllDepartmentUsers(string AccessToken)
        {
            //获取用户列表
            var param = new Dictionary<string, string>()
            {
                ["access_token"] = AccessToken,
                ["department_id"] = "1",  //从根部门开始
                ["fetch_child"] = "1"    //递归查询部门下的所有用户
            };
            var userList = await $"{_configuration["QYWechatConfiguration:DepartmentUserUrl"]}?{param.ToQueryString()}".PostAsAsync<QYWechatDepartmentUserList>();

            //循环新增每一个用户
            userList.UserList.ToList().ForEach(u =>
            {
                var user = _sysUserRep.DetachedEntities.IgnoreQueryFilters().Where(x => x.Account.Equals(u.Account) || x.Phone.Equals(u.Phone)
                || x.Email.Equals(u.Email)).FirstOrDefault();
                QYWechatRegister(u, user);
            });
        }
    }
}