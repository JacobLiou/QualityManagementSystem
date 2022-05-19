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

namespace QMS.Application.System
{
    /// <summary>
    /// 企业微信扫码登录
    /// </summary>
    public class QYWeChatOAuth : IQYWeChatOAuth, ITransient
    {
        private readonly ICacheService<string> _cache;
        private readonly IRepository<SysUser> _sysUserRep;  // 用户表仓储
        private readonly IRepository<SysOauthUser> _sysOauthUserRep;  // oauth用户表仓储
        private readonly IRepository<SysEmp> _sysEmpRep;  // 职员表仓储
        private readonly ILoginVerify _login;

        //企业微信重定向至登录界面
        private readonly string RedirectUrl = "https://open.work.weixin.qq.com/wwopen/sso/qrConnect";

        //企业微信获取access_token信息
        private readonly string AccessTokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken";

        //获取扫码用户ID
        private readonly string UserIdUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo";

        //获取扫码用户信息
        private readonly string UserInfoUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/get";

        //企业微信发送信息URL
        private readonly string SendMessageUrl = " https://qyapi.weixin.qq.com/cgi-bin/message/send";

        private readonly string Appid = "ww44dc5ededfe4a954";
        private readonly string Corpsecret = "4-WoTdHkSNnUbnpxhI3PT1pAZTRmerr9AtsMV-HweDY";
        private readonly string Agentid = "1000017";
        private readonly string Status = "web_login@gyoss9";
        private readonly long DefaultOrgId = 142307070910547;
        private readonly string DefaultOrgName = "首航新能源";

        //private readonly string LoginUrl = "http%3A%2F%2Fqms.sofarsolar.com:8001";
        private readonly string LoginUrl = "http://qms.sofarsolar.com:8001/system/qyWechat/loginAndRegister";

        private readonly int CacheHour = 2;

        public QYWeChatOAuth(ICacheService<string> cache, IRepository<SysUser> user, IRepository<SysOauthUser> oauthUser, IRepository<SysEmp> emp,
            ILoginVerify loginVerify)
        {
            _cache = cache;
            _sysUserRep = user;
            _sysOauthUserRep = oauthUser;
            _sysEmpRep = emp;
            _login = loginVerify;
        }

        /// <summary>
        /// 发起授权
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizeUrl()
        {
            var param = new Dictionary<string, string>()
            {
                ["appid"] = Appid,
                ["agentid"] = Agentid,
                ["redirect_uri"] = LoginUrl,
                ["state"] = Status
            };
            return $"{RedirectUrl}?{param.ToQueryString()}#wechat_redirect";
        }

        /// <summary>
        /// 获取企业微信Token
        /// </summary>
        /// <returns></returns>
        public async Task<QYTokenModel> GetAccessTokenAsync()
        {
            QYTokenModel accessTokenModel = new QYTokenModel();
            var result = _cache.GetCache(Appid).Result;
            if (!string.IsNullOrEmpty(result))
            {
                accessTokenModel.ErrMsg = "ok";
                accessTokenModel.ErrorCode = 0;
                accessTokenModel.AccessToken = result;
                return accessTokenModel;
            }
            var param = new Dictionary<string, string>()
            {
                ["corpid"] = Appid,
                ["corpsecret"] = Corpsecret
            };
            accessTokenModel = await $"{AccessTokenUrl}?{param.ToQueryString()}".GetAsAsync<QYTokenModel>();
            if (accessTokenModel.HasError())
                throw Oops.Oh($"{ accessTokenModel.ErrMsg}");
            await _cache.SetCacheByHours(Appid, accessTokenModel.AccessToken, CacheHour);
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
            var userIdModel = await $"{UserIdUrl}?{param.ToQueryString()}".GetAsAsync<QYUserIdModel>();
            if (userIdModel.HasError())
                throw Oops.Oh($"{ userIdModel.ErrMsg}");
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
            var userInfoModel = await $"{UserInfoUrl}?{paramUserInfo.ToQueryString()}".GetAsAsync<QYUserInfoModel>();
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
                NewEmp.OrgId = DefaultOrgId;        //深圳首航默认ID
                NewEmp.OrgName = DefaultOrgName;
                _sysEmpRep.InsertNow(NewEmp);
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
            var tourIds = _sysOauthUserRep.DetachedEntities.Where(u => touser.Contains(u.OpenId)).Select(u => u.Uuid).ToList();
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
            message.Agentid = Convert.ToInt32(Agentid);
            var messResule = await $"{SendMessageUrl}?access_token={token}".SetBody(JSON.Serialize(message)).PostAsAsync<QYWechatResult>();
            if (messResule.Errcode != 0)
                throw Oops.Oh($"{messResule.Errmsg}");
            return $"发送信息成功,消息ID为{messResule.Msgid}";
        }
    }
}