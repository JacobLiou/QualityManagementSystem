using Furion.Extras.Admin.NET;

namespace QMS.Application.System
{
    public interface ILoginVerify
    {
        string Login(SysUser user);
    }
}