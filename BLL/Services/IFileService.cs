using BLL.DTO;

namespace BLL.Services;

public interface IFileService
{
    Task<FileModel?> GetFileByIdAsync(int id);
    Task<IEnumerable<FileModel>> GetAllFilesAsync();
    Task AddFileAsync(FileModel file);
    Task DeleteFileAsync(int id);
    Task MoveFileAsync(int fileId, int folderId);
}