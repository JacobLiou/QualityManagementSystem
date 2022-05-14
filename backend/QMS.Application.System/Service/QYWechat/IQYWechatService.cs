using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.System
{
    public interface IQYWechatService
    {
        Task<dynamic> GetWechatUserInfo([FromQuery] string token, [FromQuery] string userId);

        string QYWechatLogin();

        Task QYWechatLoginCallback([FromQuery] string error_description = "");

        string QYWechatLoginRegister(string code);

        string QYWechatSendMessage(string[] touser, string toparty, string totag, string title, string description, string url);
    }
}