using Microsoft.AspNetCore.Mvc;

namespace QMS.Application.System
{
    public interface IQYWechatService
    {
        Task<dynamic> GetWechatUserInfo([FromQuery] string token, [FromQuery] string userId);
        Task QYWechatLogin();
        Task QYWechatLoginCallback([FromQuery] string error_description = "");
        string QYWechatLoginRegister(string code);
    }
}