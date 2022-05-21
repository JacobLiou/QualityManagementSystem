﻿using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core;

namespace QMS.Application.System.Service.Version
{
    /// <summary>
    /// 版本表维护接口
    /// </summary>
    public class VersionServices : IDynamicApiController, ITransient, IVersionServices
    {
        public readonly IRepository<SysVersion> _sysVersion;

        public VersionServices(IRepository<SysVersion> sysVersion)
        {
            _sysVersion = sysVersion;
        }

        /// <summary>
        /// 获取版本表全部数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("system/version/getalltypelist")]
        public async Task<List<SysVersion>> GetAllTypeList()
        {
            return await _sysVersion.DetachedEntities.Where(u => u.IsDeleted == false).ToListAsync();
        }

        /// <summary>
        /// 获取版本类别列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("system/version/gettypeversionlist")]
        public async Task<List<SysVersion>> GetTypeVersionList(string type)
        {
            return await _sysVersion.DetachedEntities.Where(u => u.Type.Equals(type) && u.IsDeleted == false).ToListAsync();
        }

        /// <summary>
        /// 新增版本号
        /// </summary>
        /// <param name="type"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpPost("system/version/addversion")]
        public async Task AddVersion(string type, string version)
        {
            var ver = _sysVersion.DetachedEntities.Where(u => u.Type.Equals(type) && u.Version.Equals(version)).FirstOrDefault();
            if (ver == null)
            {
                SysVersion newVer = new SysVersion();
                newVer.Type = type;
                newVer.Version = version;
                await _sysVersion.InsertNowAsync(newVer);
            }
        }

        /// <summary>
        /// 删除版本信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpPost("system/version/deleteversion")]
        public async Task DeleteVersion(string type, string version)
        {
            var ver = _sysVersion.DetachedEntities.Where(u => u.Type.Equals(type) && u.Version.Equals(version)).FirstOrDefault();
            if (ver != null)
            {
                ver.IsDeleted = true;
                await _sysVersion.UpdateNowAsync(ver);
            }
        }

        /// <summary>
        /// 修改版本信息
        /// </summary>
        /// <param name="id">版本记录ID</param>
        /// <param name="type">类别新值</param>
        /// <param name="version">版本号新值</param>
        /// <returns></returns>
        [HttpPost("system/version/updateversion")]
        public async Task UpdateVersion(long id, string type, string version)
        {
            var ver = _sysVersion.DetachedEntities.Where(u => u.Id == id && u.IsDeleted == false).FirstOrDefault();
            if (ver != null)
            {
                ver.Version = version;
                ver.Type = type;
                await _sysVersion.UpdateNowAsync(ver);
            }
        }
    }
}