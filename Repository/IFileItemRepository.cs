using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFileItemRepository
    {
        Task AddFileItemAsync(FileItem fileItem);
        Task<FileItem> GetFileItemByNameAsync(string fileName);
        Task<FileItem> GetByIdAsync(int id);
        Task<IEnumerable<FileItem>> GetAllAsync();
        Task AddAsync(FileItem fileItem);
        Task DeleteAsync(int id);
        Task MoveAsync(int fileId, int folderId);
    }
}