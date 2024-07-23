using BLL.DTO;

namespace BLL.Services;

public interface IFileItemService
{
    Task<FileModel> GetFileByIdAsync(int id);
    Task<IEnumerable<FileModel>> GetAllFilesAsync();
    Task AddFileAsync(FileModel file);
    Task DeleteFileAsync(int id);
    Task MoveFileAsync(int fileId, int folderId);
    Task UploadFileAsync(IFormFile file, int folderId);
    Task<byte[]> DownloadFileAsync(int id);
}
