using Furion.Extras.Admin.NET.Service;
using QMS.Application.Issues.IssueService.Dto.Field;
using QMS.Core.Enum;

namespace QMS.Application.IssueService
{
    public interface IMyIssueFieldsService
    {
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="fileds"></param>
        /// <returns></returns>
        Task AddFieldStruct(long creatorId, EnumModule module, List<FieldStruct> fileds);

        /// <summary>
        /// 添加问题字段值
        /// </summary>
        /// <param name="IssueId"></param>
        /// <param name="fieldValues"></param>
        /// <returns></returns>
        Task AddFieldValue(long IssueId, List<FieldValue> fieldValues);

        Task Delete(BaseId input);

        Task UpdateFieldStruct(EnumModule module, List<FieldStruct> fieldValues);

        Task UpdateFieldValue(List<FieldValue> fieldValues);
    }
}