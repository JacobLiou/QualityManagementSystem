using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.RemoteRequest.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.Service.ThirdPartyService.Dto;
using QMS.Core;

namespace QMS.Application.Issues
{ 
    [ApiDescriptionSettings("问题管理服务", Name = "ThirdParty", Order = 100)]

    public class ThirdPartyService : IDynamicApiController, IScoped
    {
        private readonly IHttpProxy _http;
        private readonly IHttpContextAccessor _contextAccessor;
        public ThirdPartyService(IHttpProxy http, IHttpContextAccessor contextAccessor)
        {
            _http = http;
            _contextAccessor = contextAccessor;
        }

        [NonAction]
        public async Task<Dictionary<long, ProjectModelFromThirdParty>> GetProjectByIds(IEnumerable<long> projectIds)
        {
            var request = _contextAccessor.HttpContext.Request;
            var authHeader = request.Headers["Authorization"];

            var response = 
                await Constants.PROJECT_URL
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(projectIds)
                .PostAsAsync<ThirdPartyApiModel<Dictionary<long, ProjectModelFromThirdParty>>>();

            return response.data;
        }

        [NonAction]
        public async Task<Dictionary<long, ProductModelFromThirdParty>> GetProductByIds(IEnumerable<long> projectIds)
        {
            var request = _contextAccessor.HttpContext.Request;
            var authHeader = request.Headers["Authorization"];

            var response =
                await Constants.PRODUCT_URL
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(projectIds)
                .PostAsAsync<ThirdPartyApiModel<Dictionary<long, ProductModelFromThirdParty>>>();

            return response.data;
        }
    }

    public class ThirdPartyApiModel<T>
    {
        public bool success { get; set; }
        public int code { get; set; }
        public T data { get; set; }
        public string message { get; set; }
    }
}
