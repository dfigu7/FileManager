using DataAccess.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFileItemRepository
    {
        Task<FileItem> AddAsync(FileItem fileItem);
        Task AddFileItemAsync(FileItem fileItem);
        Task<FileItem> GetFileItemByNameAsync(string fileName);

        Task<IEnumerable<FileItem>> GetAllAsync();
        Task<bool> FileExistsAsync(string name, int folderId);
        Task DeleteAsync(int id);
        Task MoveAsync(int fileId, int folderId);
        Task<FileItem> GetByIdAsync(int id);
        Task UpdateAsync(FileItem file);
        Task<string> GetFullPath(int folderId);
        Task<IEnumerable<FileItem>> SearchByNameAsync(string name);
        Task<IEnumerable<FileItem>> GetFilesByFolderIdAsync(int folderId);
        object GetFilesByFolderId(int folderId);
        Task<IEnumerable<FileItem>> GetFilesByDateAsync(DateTime date);
    }
}