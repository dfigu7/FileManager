using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.DTO;

namespace BLL.Services
{
    public interface IFileItemService{
        Task AddFileItemAsync(FileItem fileItem);
        Task<FileItem> GetFileItemByNameAsync(string fileName);

        Task<FileModel> GetByIdAsync(int id);
        Task<IEnumerable<FileModel>> GetAllAsync();
        Task<bool> FileExistsAsync(string name, int folderId);
        Task<bool>DeleteFileAsync(int id);
        Task<bool> MoveFileAsync(int fileId, int FolderId);
        Task AddFileAsync(FileItem fileModel);
    }
}