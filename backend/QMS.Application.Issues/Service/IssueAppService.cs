using Furion.DynamicApiController;
using Furion.RemoteRequest.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Application.Issues
{ /// <summary>
  /// 系统服务接口
  /// </summary>
    [ApiDescriptionSettings("问题管理", Name = "Issue", Order = 100)]

    public class IssueAppService : IDynamicApiController
    {
        private readonly IHttpProxy _http;
        private readonly IHttpContextAccessor _contextAccessor;
        public IssueAppService(IHttpProxy http, IHttpContextAccessor contextAccessor)
        {
            _http = http;
            _contextAccessor = contextAccessor;
        }
        [HttpGet("/Issue/TestUserGroup")]
        public async Task<List<GroupUserOutput>> GetUserGroup()
        {
            return await this.GroupUserOutputs();
        }

        public async Task<List<GroupUserOutput>> GroupUserOutputs()
        {
            var request = _contextAccessor.HttpContext.Request;
            var authHeader = request.Headers["Authorization"];

            var response = await "http://localhost:5566/System/UserGroup".SetHeaders(new
            {
                Authorization = authHeader
            }).GetAsStringAsync();

            var context = JsonConvert.DeserializeObject<ApiModel<List<GroupUserOutput>>>(response);
            return context.data;

        }

    }

    public class ApiModel<T>
    {
        public bool success { get; set; }
        public int code { get; set; }
        public T data { get; set; }
        public string message { get; set; }
    }
}
