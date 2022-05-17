using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.System
{
    public interface IQYWechatService
    {
        Task<dynamic> GetWechatUserInfo([FromQuery] string token, [FromQuery] string userId);

        Task QYWechatLogin();

        Task QYWechatLoginCallback([FromQuery] string error_description = "");

        Task QYWechatLoginRegister(string code);

        string QYWechatGetLoginToken(string code);

        string QYWechatSendMessage(string[] touser, string toparty, string totag, string title, string description, string url);
    }
}