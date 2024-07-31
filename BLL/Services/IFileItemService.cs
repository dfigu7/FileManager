using BLL.DTO;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IFileItemService{
        Task AddFileItemAsync(FileItem fileItem);
        Task<FileItem> GetFileItemByNameAsync(string fileName);

        Task<FileModel> GetByIdAsync(int id);
        Task<IEnumerable<FileModel>> GetAllAsync();
        Task<bool> FileExistsAsync(string name, int folderId);
        Task DeleteFileAsync(int id);
        Task MoveFileAsync(int fileId, int folderId);
        Task AddFileAsync(FileItem fileModel);
    }
}