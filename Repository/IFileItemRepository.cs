using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFileItemRepository
    {
        Task<FileItem> AddAsync(FileItem fileItem);
        Task AddFileItemAsync(FileItem fileItem);
        Task<FileItem> GetFileItemByNameAsync(string fileName);
        Task<FileItem> GetByIdAsync(int id);
        Task<IEnumerable<FileItem>> GetAllAsync();
        Task<bool> FileExistsAsync(string name, int folderId);
        Task DeleteAsync(int id);
        Task MoveAsync(int fileId, int folderId);
        Task<IEnumerable<FileItem>> SearchByNameAsync(string name);
    }
}