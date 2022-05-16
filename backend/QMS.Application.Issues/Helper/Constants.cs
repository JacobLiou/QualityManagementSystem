namespace QMS.Application.Issues.Helper
{
    internal class Constants
    {
        public readonly static string UPLOAD_FILE = "http://localhost:5566/sysFileInfo/upload";

        public readonly static string DOWNLOAD_FILE = "http://localhost:5566/sysFileInfo/download";

        public readonly static string PROJECT_URL = "http://localhost:5566/SsuProject/list";

        public readonly static string PRODUCT_URL = "http://localhost:5566/SsuProduct/list";

        public const string USER_COLUMNS = "UserColumns_";

        public readonly static Dictionary<string, string> USER_COLUMN_NAMES = new Dictionary<string, string>
        {
            {"title", "标题"},
            {"projectName", "项目名"},
            {"productName", "产品名"},
            {"module", "问题模块"},
            {"consequence", "问题性质"},
            {"issueClassification", "问题分类"},
            {"source", "问题来源"},
            {"status", "问题状态"},
            {"creatorName", "提出人"},
            {"createTime", "提出日期"},
            //{"closeTime", "关闭日期"},
            {"discoverName", "发现人"},
            {"discoverTime", "发现日期"},
            {"dispatcherName", "分发人"},
            {"dispatchTime", "分发日期"},
            //{"forecastSolveTime", "预计完成日期"},
            //{"copyToName", "被抄送人"},
            {"executorName", "解决人"},
            //{"verifierName", "验证人"},
            //{"verifierPlace", "验证地点"},
            //{"validateTime", "验证日期"}
        };

        public const string ERROR_MSG_CHECK_SOLVE = "当前问题未指定负责人处理，不允许进行处理操作";
        public const string ERROR_MSG_CHECK_VALIDATE = "当前问题不是已处理状态，不允许进行验证操作";
        public const string ERROR_MSG_CHECK_HANGUP = "当前问题已关闭，不允许进行挂起操作";
        public const string ERROR_MSG_CHECK_REDISPATCH = "当前问题为已开启或已挂起、关闭，不允许进行重分派操作";
        public const string ERROR_MSG_CHECK_DISPATCH = "当前问题不为已开启或已挂起，不允许进行分派操作";
    }
}
