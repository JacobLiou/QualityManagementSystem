﻿namespace QMS.Application.System
{
    public interface IRegisterService
    {
        bool RegisterUser(RegisterInput input);

        string SendSMSCode(string phone, int num = 4);
    }
}