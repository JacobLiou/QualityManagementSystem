using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Furion.Extras.Admin.NET.Service
{
    public interface ISysFileService
    {
        Task DeleteFileInfo(DeleteFileInfoInput input);

        Task<IActionResult> DownloadFileInfo([FromQuery] QueryFileInfoInput input);

        Task<SysFile> GetFileInfo([FromQuery] QueryFileInfoInput input);

        Task<List<SysFile>> GetFileInfoList([FromQuery] FileOutput input);

        Task<IActionResult> PreviewFileInfo([FromQuery] QueryFileInfoInput input);

        Task<PageResult<FileOutput>> QueryFileInfoPageList([FromQuery] FilePageInput input);

        Task<long> UploadFileAvatar(IFormFile file);

        Task<long> UploadFileDefault(IFormFile file);

        Task UploadFileDocument(IFormFile file);

        Task UploadFileShop(IFormFile file);

        Task<long> UploadFile(IFormFile file, string key);
    }
}