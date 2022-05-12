namespace QMS.Application.Issues.Helper
{
    internal class Constants
    {
        public const string UPLOAD_FILE = "http://localhost:5566/sysFileInfo/upload";

        public const string DOWNLOAD_FILE = "http://localhost:5566/sysFileInfo/download";

        public const string USER_COLUMNS = "UserColumns_";

        public static readonly KeyValuePair<string, string>[] USER_COLUMN_NAMES = new KeyValuePair<string, string>[]
        {
            new ("title", "标题"),
            new ("projectName", "项目名"),
            new ("productName", "产品名"),
            new ("module", "问题模块"),
            new ("consequence", "问题性质"),
            new ("issueClassification", "问题分类"),
            new ("source", "问题来源"),
            new ("status", "问题状态"),
            new ("creatorName", "提出人"),
            new ("createTime", "提出日期"),
            new ("closeTime", "关闭日期"),
            new ("discoverName", "发现人"),
            new ("discoverTime", "发现日期"),
            new ("dispatcherName", "分发人"),
            new ("dispatchTime", "分发日期"),
            new ("forecastSolveTime", "预计完成日期"),
            new ("copyToName", "被抄送人"),
            new ("executorName", "解决人"),
            new ("verifierName", "验证人"),
            new ("verifierPlace", "验证地点"),
            new ("validateTime", "验证日期")
        };
    }
}
