using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.Extras.Admin.NET.Entity.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.System.Service
{
    [AllowAnonymous]
    public class MonitorAppService : IDynamicApiController, ITransient
    {
        private readonly IRepository<MonitorUser> _monitorUser; // 用户表
        private readonly IRepository<MonitorCode> _monitorCode;  // 注册码
        public MonitorAppService(IRepository<MonitorUser> monitorUser
            , IRepository<MonitorCode> monitorCode)
        {
            _monitorUser = monitorUser;
            _monitorCode = monitorCode;
        }
        [HttpGet("system/monitorApp/register")]
        public string RegisterMonitor(string code, string guid, string name)
        {

            var data = _monitorCode.FirstOrDefault(x => x.Code == code);
            if (data == null)
            {
                return null;
            }
            MonitorUser user = new MonitorUser();
            user.Name = name;
            user.Code = code;
            user.Machine = guid;
            user.CreatedTime = DateTime.Now;
            var insertUser = _monitorUser.Insert(user);
            if (insertUser == null)
            {
                return null;
            }
            guid = Decrypt(guid);
            string check = Encrypt(guid + "|" + data.Role);

            return check;
        }


        //默认密钥向量 
        private byte[] iv = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        //key
        private string key = "license_sofar_g3";

        // DES加密和解密
        private string Encrypt(string encryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 16));
                byte[] rgbIV = iv;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

        
                var DCSP = Aes.Create();
                DCSP.Padding = PaddingMode.PKCS7;
                DCSP.Mode = CipherMode.ECB;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message + encryptString;
            }
        }
        private string Decrypt(string decryptString)
        {
            try
            {

                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 16));
                byte[] rgbIV = iv;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                var DCSP = Aes.Create();

                DCSP.Padding = PaddingMode.PKCS7;
                DCSP.Mode = CipherMode.ECB;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                Byte[] inputByteArrays = new byte[inputByteArray.Length];
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message + decryptString;
            }
        }


    }
}
