using Furion;
using Furion.DatabaseAccessor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using QMS.Application.Issues.Service.Issue.Dto.Add;
using QMS.Core;
using QMS.Core.Entity;
using System.Text;
using System.Web;

namespace QMS.Application.Issues.Helper
{
    internal static partial class Helper
    {
        #region Download、Upload

        public static async Task<IActionResult> ExportExcel(object data, string fileName = null)
        {
            Assert(data != null, "数据为空，无法下载文件!");

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(data);
            memoryStream.Seek(0, SeekOrigin.Begin);

            fileName = fileName ?? DateTime.Now.ToString("yyyyMMddHHmmss");

            return await Task.FromResult(
                new FileStreamResult(memoryStream, "application/octet-stream")
                {
                    FileDownloadName = HttpUtility.UrlEncode(fileName + ".xlsx", Encoding.GetEncoding("UTF-8"))
                });
        }

        #endregion Download、Upload

        #region Assert

        public static async Task<Issue> CheckIssueExist(IRepository<Issue, IssuesDbContextLocator> issueRep, long id)
        {
            Issue issue = await issueRep.DetachedEntities.FirstOrDefaultAsync(issue => issue.Id == id);

            Assert(issue != null, $"问题【{id}】不存在");

            return issue;
        }

        public static async Task<IssueDetail> CheckIssueDetailExist(IRepository<IssueDetail, IssuesDbContextLocator> issueDetailRep, long id)
        {
            IssueDetail issue = await issueDetailRep.DetachedEntities.FirstOrDefaultAsync(issue => issue.Id == id);

            Assert(issue != null, $"问题【{id}】详情不存在");

            return issue;
        }

        public static void CheckInput(object input)
        {
            Helper.Assert(input != null, "接口需要的传入参数为null");
        }

        /// <summary>
        /// 判断是否已经存在相同记录的问题
        /// </summary>
        /// <param name="issueRep"></param>
        /// <param name="input"></param>
        public static void CheckRepeatInput(IRepository<Issue, IssuesDbContextLocator> issueRep, InIssue input)
        {
            var issue = issueRep.DetachedEntities.FirstOrDefault(u => u.ProductId == input.ProductId && u.ProjectId == input.ProjectId
            && u.Module == input.Module && u.IssueClassification == input.IssueClassification
            && u.CurrentAssignment == input.CurrentAssignment && u.Consequence == input.Consequence
            && u.Title == input.Title);
            if (issue != null)
            {
                throw new ArgumentException($"已存在序号为:{issue.SerialNumber}的数据相同的数据行");
            }
        }

        public static void Assert(bool result, string errorMsg = "参数错误")
        {
            if (!result)
            {
                throw new ArgumentException(errorMsg);
            }
        }

        public static void Assert(bool result, Exception exception)
        {
            if (!result)
            {
                throw exception;
            }
        }

        public static void Assert(string param, Predicate<string> predicate, string errorMsg = "参数错误")
        {
            if (predicate != null && !predicate.Invoke(param))
            {
                throw new ArgumentException(errorMsg);
            }
        }

        #endregion Assert

        #region 日期转换

        public static DateTime GetDateTime(this string time)
        {
            //Assert(time != null);

            return Convert.ToDateTime(time);
        }

        public static string GetDateString(this DateTime time)
        {
            //Assert(time != null);

            return time.ToString("yyyy-MM-dd");
        }

        public static string GetDateString(this DateTime? time)
        {
            //Assert(time != null);

            return time?.ToString("yyyy-MM-dd");
        }

        public static string GetTimeString(this DateTime time)
        {
            //Assert(time != null);

            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetTimeString(this DateTime? time)
        {
            //Assert(time != null);

            return time?.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion 日期转换

        #region 第三方服务接口

        public static ThirdPartyService GetThirdPartyService()
        {
            return App.GetService<ThirdPartyService>();
        }

        #endregion 第三方服务接口
    }
}