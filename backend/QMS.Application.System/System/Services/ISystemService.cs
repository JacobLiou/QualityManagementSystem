using QMS.Application.System.Service;

namespace QMS.Application.System
{
    public interface ISystemService
    {
        string GetDescription();

        List<GroupUserOutput> GetUserGroup();

        List<SsuGroupOutput> GetGroup();
    }
}