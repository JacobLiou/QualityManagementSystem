using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.Extras.Admin.NET;
using Furion.FriendlyException;
using System.Net;
using System.Text;

namespace QMS.Application.System
{
    /// <summary>
    /// 手机短信验证码服务
    /// </summary>
    public class PhoneVerify : ITransient, IPhoneVerify
    {
        private readonly ICacheService _cache;
        private readonly IRepository<SysUser> _sysUserRep; // 用户表仓储
        private readonly ILoginVerify _login;
        private readonly int CacheMinute = 1;
        private readonly string PostUrl = "https://sdk2.028lk.com/sdk2/BatchSend2.aspx"; //短信网关接口地址
        private readonly string CorpID = "GZJS001651";           //接口账号
        private readonly string Pwd = "sh@668";                  //接口密码
        private readonly string Context = "您好，您的验证码是：{0}【首航新能源】";    //手机验证码格式

        public PhoneVerify(ICacheService cache, IRepository<SysUser> sysUser, ILoginVerify loginVerify)
        {
            _cache = cache;
            _sysUserRep = sysUser;
            _login = loginVerify;
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="num">验证码个数</param>
        /// <returns></returns>
        public string SendSMSCode(string phone, int num = 4)
        {
            if (num <= 0)
            {
                num = 4;
            }
            string code = GetRandomNums(num);
            string context = String.Format(Context, code);
            CommonOutput output = SendSMS(phone, context);
            if (!output.Success)
            {
                throw Oops.Oh(output.Message);
            }
            return output.Message;
        }

        /// <summary>
        /// 生成一定长度的随机数字串，默认值为4位
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetRandomNums(int length = 4)
        {
            var chars = new StringBuilder();
            char[] character = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
            Random rnd = new();
            // 生成验证码字符串
            for (int i = 0; i < length; i++)
            {
                chars.Append(character[rnd.Next(character.Length)]);
            }
            _cache.SetCacheByMinutes(CacheKeys.CACHE_PHONE_CODE, chars.ToString(), CacheMinute);   //设置缓存时间为一分钟
            return chars.ToString();
        }

        /// <summary>
        /// 验证随机字符串数字是否正确
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public CommonOutput VerifyPhoneNums(string nums)
        {
            CommonOutput output = new CommonOutput();
            var code = _cache.GetCache<string>(CacheKeys.CACHE_PHONE_CODE);     //获取缓存
            if (code == null || code.Result == null || string.IsNullOrEmpty(code.Result))
            {
                output.Success = false;
                output.Message = "验证码已过期请重新生成";
                return output;
            }
            if (!code.Result.Equals(nums))
            {
                output.Success = false;
                output.Message = "验证码错误，请重新输入";
                return output;
            }
            output.Success = true;
            return output;
        }

        /// <summary>
        /// Post提交请求
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string PostSMS(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("GB2312");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {                // 设置参数
                HttpWebRequest request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();                 //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;//直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, encoding);                 //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
            finally
            {
                if (outstream != null)
                {
                    outstream.Dispose();
                }
                if (instream != null)
                {
                    instream.Dispose();
                }
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="context">短信内容</param>
        /// <returns></returns>
        public CommonOutput SendSMS(string mobile, string context)
        {
            int reuslt;
            CommonOutput output = new CommonOutput();
            output.Success = false;

            string Postdate = "CorpID=" + CorpID + "&Pwd=" + Pwd + "&Mobile=" + mobile + "&Content=" + context + "&Cell=&SendTime=";
            int.TryParse(PostSMS(PostUrl, Postdate), out reuslt);
            switch (reuslt)
            {
                case > 0:
                    output.Success = true;
                    output.Message = "提交短信成功！！返回值为：" + reuslt;
                    break;

                case 0:
                    output.Message = "调用接口失败了，没有返回值";
                    break;

                case -1:
                    output.Message = "账号未注册！返回值为：" + reuslt;
                    break;

                case -2:
                    output.Message = "网络延迟或参数填写不正确！返回值为：" + reuslt;
                    break;

                case -3:
                    output.Message = "账号或密码错误！返回值为：" + reuslt;
                    break;

                case -5:
                    output.Message = "短信余额不足！请联系客服充值！返回值为：" + reuslt;
                    break;

                case -6:
                    output.Message = "SendTime定时短信参数格式不正确：" + reuslt;
                    break;

                case -7:
                    output.Message = "签名错误，请在短信内容末尾加上签名！返回值为：" + reuslt;
                    break;

                case -8:
                    output.Message = "发送内容需在1到300字之间！：" + reuslt;
                    break;

                case -9:
                    output.Message = "手机号为空！返回值为：" + reuslt;
                    break;

                case -10:
                    output.Message = "定时时间不能小于系统当前时间！返回值为：" + reuslt;
                    break;

                case -100:
                    output.Message = "IP黑名单！返回值为：" + reuslt;
                    break;

                case -102:
                    output.Message = "账号黑名单！返回值为：" + reuslt;
                    break;

                case -103:
                    output.Message = "IP未导白！返回值为：" + reuslt;
                    break;

                default:
                    output.Message = "其他错误！调用返回：" + reuslt;
                    break;
            }
            return output;
        }


        /// <summary>
        /// 手机号登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        public string phoneLogin(string phone, string captcha)
        {
            if (string.IsNullOrEmpty(captcha.Trim()))
            {
                throw Oops.Oh($"验证码为空请重新输入");
            }
            //判断验证码是否正确
            CommonOutput output = VerifyPhoneNums(captcha);
            if (!output.Success)
            {
                throw Oops.Oh($"{output.Message}");
            }
            var user = _sysUserRep.Where(u => u.Phone.Equals(phone)).FirstOrDefault();
            if (user == null)
            {
                throw Oops.Oh($"该用户不存在，请注册用户");
            }
            //登录
            return _login.Login(user);
        }
    }
}