// FileManager.BLL/Services/IFileService.cs

using BLL.Models;

namespace BLL.Services;

public interface IFileService
{
    Task<FileModel> GetFileByIdAsync(int id);
    Task<IEnumerable<FileModel>> GetAllFilesAsync();
    Task AddFileAsync(FileModel file);
    Task DeleteFileAsync(int id);
    Task MoveFileAsync(int fileId, int folderId);
}