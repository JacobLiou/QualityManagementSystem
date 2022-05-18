using Furion.Extras.Admin.NET;

namespace QMS.Application.System
{
    public interface IQYWeChatOAuth
    {
        Task<QYTokenModel> GetAccessTokenAsync();

        string GetAuthorizeUrl();

        Task<QYUserIdModel> GetQYUserIdAsync(string accessToken, string code);

        Task<QYUserInfoModel> GetQYUserInfoAsync(string accessToken, string userId);

        Task<string> QYWechatSendMessage(string[] touser, string toparty, string totag, string title, string description, string url);

        SysUser QYWechatRegister(QYUserInfoModel qYUserInfo, SysUser sysUser);

        string QYWechatLogin(SysUser user);
    }
}