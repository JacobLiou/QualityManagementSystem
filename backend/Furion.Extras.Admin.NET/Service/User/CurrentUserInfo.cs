namespace Furion.Extras.Admin.NET
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public static class CurrentUserInfo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public static long UserId => App.User != null ? long.Parse(App.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value) : 0;

        /// <summary>
        /// 账号
        /// </summary>
        public static string Account => App.User != null ? App.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value : "";

        /// <summary>
        /// 昵称
        /// </summary>
        public static string Name => App.User != null ? App.User.FindFirst(ClaimConst.CLAINM_NAME)?.Value : "";

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public static bool IsSuperAdmin => App.User != null ? App.User.FindFirst(ClaimConst.CLAINM_SUPERADMIN)?.Value == ((int)AdminType.SuperAdmin).ToString() : false;
    }
}