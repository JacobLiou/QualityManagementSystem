using Furion.RemoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core
{
    public interface IHttpProxy:IHttpDispatchProxy
    {
        /// <summary>
        /// Headers("Authorization",app.c)
        /// </summary>
        /// <returns></returns>
        [Get("http://localhost:5566/System/UserGroup")]
        Task<List<GroupUserOutput>> GetUserGroupAsync();


    }
}
