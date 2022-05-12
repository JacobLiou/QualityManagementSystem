
namespace QMS.Application.System
{
    public interface IQYWeChatOAuth
    {
        Task<QYTokenModel> GetAccessTokenAsync();
        string GetAuthorizeUrl();
        Task<QYUserIdModel> GetQYUserIdAsync(string accessToken, string code);
        Task<QYUserInfoModel> GetQYUserInfoAsync(string accessToken, string userId);
    }
}