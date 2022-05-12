namespace QMS.Application.Issues.Helper
{
    internal class Constants
    {
        public const string UPLOAD_FILE = "http://localhost:5566/sysFileInfo/upload";

        public const string DOWNLOAD_FILE = "http://localhost:5566/sysFileInfo/download";

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
    }
}
