using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Furion.JsonSerialization;
using Furion.RemoteRequest.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QMS.Application.Issues.Helper;
using QMS.Application.Issues.Service.ThirdPartyService.Dto;
using QMS.Core;
using Furion.Extras.Admin.NET;

namespace QMS.Application.Issues
{
    [ApiDescriptionSettings("问题管理服务", Name = "ThirdParty", Order = 100)]
    public class ThirdPartyService : IDynamicApiController, IScoped
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ThirdPartyService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 根据项目ID列表调用接口获取项目详情（调用前请根据缓存值先从缓存中获取一次数据）
        /// </summary>
        /// <param name="projectIds"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<Dictionary<long, ProjectModelFromThirdParty>> GetProjectByIds(IEnumerable<long> projectIds)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            List<Dictionary<string, long>> ids = new List<Dictionary<string, long>>();
            foreach (long id in projectIds.ToList().Distinct())
            {
                var param = new Dictionary<string, long>()
                {
                    ["Id"] = id
                };
                ids.Add(param);
            }
            var response =
                await Constants.PROJECTS_URL
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(JSON.Serialize(ids.ToArray()))
                .PostAsAsync<ThirdPartyApiModel<List<ProjectModelFromThirdParty>>>();

            //Helper.Helper.Assert(response != null && response.data != null && response.data.Count > 0, $"项目集合不存在");
            var result = response.data.ToDictionary(u => u.Id, u => u);
            return result;
        }

        /// <summary>
        /// 根据项目名称列表调用接口获取详情（调用前请根据缓存值先从缓存中获取一次数据）
        /// </summary>
        /// <param name="projectNames"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<Dictionary<string, ProjectModelFromThirdParty>> GetProjectByNames(IEnumerable<string> projectNames)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            List<Dictionary<string, string>> ids = new List<Dictionary<string, string>>();
            foreach (string name in projectNames.ToList().Distinct())
            {
                var param = new Dictionary<string, string>()
                {
                    ["ProjectName"] = name
                };
                ids.Add(param);
            }
            var response =
                await Constants.PROJECTS_URL
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(JSON.Serialize(ids.ToArray()))
                .PostAsAsync<ThirdPartyApiModel<List<ProjectModelFromThirdParty>>>();

            var result = response.data.ToDictionary(u => u.ProjectName, u => u);
            return result;
        }

        /// <summary>
        /// 根据产品ID列表调用接口获取产品详情（调用前请根据缓存值先从缓存中获取一次数据）
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<Dictionary<long, ProductModelFromThirdParty>> GetProductByIds(IEnumerable<long> productIds)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            List<Dictionary<string, long>> ids = new List<Dictionary<string, long>>();
            foreach (long id in productIds.ToList().Distinct())
            {
                var param = new Dictionary<string, long>()
                {
                    ["Id"] = id
                };
                ids.Add(param);
            }

            var response =
                await Constants.PRODUCTS_URL
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(JSON.Serialize(ids.ToArray()))
                .PostAsAsync<ThirdPartyApiModel<List<ProductModelFromThirdParty>>>();

            var result = response.data.ToDictionary(u => u.Id, u => u);
            return result;
        }

        /// <summary>
        /// 根据产品名称列表调用接口获取产品详情（调用前请根据缓存值先从缓存中获取一次数据）
        /// </summary>
        /// <param name="productNames"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<Dictionary<string, ProductModelFromThirdParty>> GetProductByNames(IEnumerable<string> productNames)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            List<Dictionary<string, string>> names = new List<Dictionary<string, string>>();
            foreach (string name in productNames.ToList().Distinct())
            {
                var param = new Dictionary<string, string>()
                {
                    ["ProductName"] = name
                };
                names.Add(param);
            }

            var response =
                await Constants.PRODUCTS_URL
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(JSON.Serialize(names.ToArray()))
                .PostAsAsync<ThirdPartyApiModel<List<ProductModelFromThirdParty>>>();

            var result = response.data.ToDictionary(u => u.ProductName, u => u);
            return result;
        }

        /// <summary>
        /// 根据目ID和模块ID调用接口获取人员列表（调用前请根据缓存值先从缓存中获取一次数据）
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="modularId"></param>
        /// <returns></returns>
        public async Task<List<UserModelFromThirdParty>> GetUserByProjectModularId(long projectId, long modularId)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            var param = new Dictionary<string, string>()
            {
                ["projectId"] = projectId.ToString(),
                ["modularId"] = modularId.ToString()
            };

            //get请求下，通过SetQueries方法设置请求参数无法正常请求，此处先暂时通过这种方式
            var response =
                await $"{Constants.PROJECT_MODULAR_URL + "?" + param.ToQueryString()}"
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .GetAsAsync<ThirdPartyApiModel<List<UserModelFromThirdParty>>>();

            return response.data;
        }

        /// <summary>
        /// 根据用户ID调用接口获取用户详情（调用前请根据缓存值先从缓存中获取一次数据）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<string> GetNameById(long userId)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            List<Dictionary<string, long>> Ids = new List<Dictionary<string, long>>();
            var param = new Dictionary<string, long>()
            {
                ["Id"] = userId
            };
            Ids.Add(param);

            var response =
                await Constants.USER_URL
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(JSON.Serialize(Ids.ToArray()))
                .PostAsAsync<ThirdPartyApiModel<List<UserModelFromThirdParty>>>();

            //Helper.Helper.Assert(response != null && response.data != null, $"员工【{userId}】不存在");

            var result = response.data.ToDictionary(u => u.Id, u => u);

            if (response.data.Count == 0)
            {
                return String.Empty;
            }

            return result[userId].Name;
        }

        /// <summary>
        /// 根据用户名称列表调用接口获取用户详情（调用前请根据缓存值先从缓存中获取一次数据）
        /// </summary>
        /// <param name="userNames"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, UserModelFromThirdParty>> GetUserByName(IEnumerable<string> userNames)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            List<Dictionary<string, string>> names = new List<Dictionary<string, string>>();
            foreach (string name in userNames.ToList().Distinct())
            {
                var param = new Dictionary<string, string>()
                {
                    ["Name"] = name
                };
                names.Add(param);
            }

            var response =
                await Constants.USER_URL
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(JSON.Serialize(names.ToArray()))
                .PostAsAsync<ThirdPartyApiModel<List<UserModelFromThirdParty>>>();

            var result = response.data.ToDictionary(u => u.Name, u => u);
            return result;
        }



        /// <summary>
        /// 根据模块值列表调用接口获取模块详情（调用前请根据缓存值先从缓存中获取一次数据）
        /// </summary>
        /// <param name="modularValue"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, ModularModelFromThirdParty>> GetModularByValue(IEnumerable<string> modularValue)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();
            foreach (string value in modularValue.ToList().Distinct())
            {
                var param = new Dictionary<string, string>()
                {
                    ["Value"] = value
                };
                values.Add(param);
            }

            var response =
                await Constants.MODULAR_URL
                //await "http://172.16.16.33:8001/dictonaryservice/getdictdetail"
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .SetBody(JSON.Serialize(values))
                .PostAsAsync<ThirdPartyApiModel<List<ModularModelFromThirdParty>>>();

            var result = response.data.ToDictionary(u => u.Value, u => u);
            return result;
        }

        /// <summary>
        /// 根据字典Type类型获取该类型下的所有值
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public async Task<List<DictDataFromThridParty>> GetDictDataByCode(string typeCode)
        {
            var authHeader = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            var param = new Dictionary<string, string>()
            {
                ["Code"] = typeCode
            };

            //get请求下，通过SetQueries方法设置请求参数无法正常请求，此处先暂时通过这种方式
            var response =
                await $"{Constants.DICT_DATA_URL + "?" + param.ToQueryString()}"
                .SetHeaders(new
                {
                    Authorization = authHeader
                })
                .GetAsAsync<ThirdPartyApiModel<List<DictDataFromThridParty>>>();

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