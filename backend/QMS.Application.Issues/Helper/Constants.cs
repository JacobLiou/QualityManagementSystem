using QMS.Core.Enum;

namespace QMS.Application.Issues.Helper
{
    internal class Constants
    {
        public static readonly string URL_ROOT = "http://qms.sofarsolar.com:8001/";

        public static readonly string UPLOAD_FILE = Constants.URL_ROOT + "sysFileInfo/upload";

        public static readonly string DOWNLOAD_FILE = Constants.URL_ROOT + "sysFileInfo/download";

        public static readonly string PROJECTS_URL = Constants.URL_ROOT + "SsuProject/getprojectlist";

        public static readonly string PRODUCTS_URL = Constants.URL_ROOT + "SsuProduct/getproductlist";

        public static readonly string USER_URL = Constants.URL_ROOT + "SsuEmpOrg/getuserlist";

        public static readonly string PROJECT_MODULAR_URL = Constants.URL_ROOT + "SsuEmpOrg/getresponsibilityuser";

        public static readonly string MODULAR_URL = Constants.URL_ROOT + "dictonaryservice/getdictdetail";

        public static readonly string DICT_DATA_URL = Constants.URL_ROOT + "sysDictType/dropDown";

        public static readonly string DOWN_FILE = URL_ROOT + "sysFileInfo/download";

        public const string PROJECT_MARK = "项目";
        public const string PRODUCT_MARK = "产品";


        public const string USER_COLUMNS = "UserColumns_";

        public static readonly Dictionary<string, string> USER_COLUMN_NAMES = new Dictionary<string, string>
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
        public const string ERROR_MSG_CHECK_VALIDATE = "当前问题不是待验证状态，不允许进行验证操作";
        public const string ERROR_MSG_CHECK_HANGUP = "当前问题已验证关闭，不允许进行挂起操作";
        public const string ERROR_MSG_CHECK_REDISPATCH = "当前问题为新增，处理中或未解决，不允许进行转交操作";
        public const string ERROR_MSG_CHECK_DISPATCH = "当前问题不为新建，挂起或未解决，不允许进行分派操作";

        public const string ERROR_MSG_CHECK_RECHECK = "当前问题不是待复核，不允许进行复核操作";


        public const string FIELD_STRUCT = "FieldStruct";

        public static readonly string TRAIL_PRODUCTION = EnumModule.TrialProduce.ToString().ToLower() + "_" + "trail_production_process";
        public static readonly string TEST_CALSSIFICATION = EnumModule.Test.ToString().ToLower() + "_" + "test_classification";
    }
}