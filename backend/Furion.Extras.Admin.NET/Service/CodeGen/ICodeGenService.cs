using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service.CodeGen
{
    public interface ICodeGenService
    {
        Task AddCodeGen(AddCodeGenInput input);

        Task DeleteCodeGen(List<DeleteCodeGenInput> inputs);

        Task<SysCodeGen> GetCodeGen([FromQuery] QueryCodeGenInput input);

        List<TableColumnOuput> GetColumnList(AddCodeGenInput input);

        List<TableOutput> GetTableList(string dbContextLocatorName);

        Task<PageResult<SysCodeGen>> QueryCodeGenPageList([FromQuery] CodeGenPageInput input);

        Task RunLocal(SysCodeGen input);

        Task UpdateCodeGen(UpdateCodeGenInput input);
    }
}