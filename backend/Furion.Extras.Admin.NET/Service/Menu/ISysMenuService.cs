﻿using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysMenuService
    {
        Task AddMenu(AddMenuInput input);

        Task<List<AntDesignTreeNode>> ChangeAppMenu(ChangeAppMenuInput input);

        Task DeleteMenu(DeleteMenuInput input);

        Task<List<AntDesignTreeNode>> GetLoginMenusAntDesign(long userId, string appCode);

        Task<List<string>> GetLoginPermissionList(long userId);

        Task<List<string>> GetAllPermissionList();

        Task<dynamic> GetMenu(QueryMenuInput input);

        Task<dynamic> GetMenuList([FromQuery] GetMenuListInput input);

        Task<dynamic> GetMenuTree([FromQuery] GetMenuTreeInput input);

        Task<List<string>> GetUserMenuAppCodeList(long userId);

        Task<bool> HasMenu(string appCode);

        Task<dynamic> TreeForGrant([FromQuery] TreeForGrantInput input);

        Task UpdateMenu(UpdateMenuInput input);
    }
}