namespace QMS.Application.System
{
    public interface IRegisterService
    {
        string GenerateCode(int num = 4);

        bool RegisterUser(RegisterInput input);

        string SendSMSCode(string phone, int num = 4);
    }
}