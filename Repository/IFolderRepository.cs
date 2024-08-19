// FileManager.DataAccess/Repositories/IFolderRepository.cs

using DataAccess.Entities;

namespace Repository;

public interface IFolderRepository : IRepository<Folder>
{
    // Additional methods specific to Folder repository
    Task<IEnumerable<Folder>> SearchByNameAsync(string name);
    Task<IEnumerable<Folder>> GetSubFoldersAsync(int folderId);
    Task UpdateAsync(Folder childFolder);
    Task<bool> DeleteAsync(int folderId);

    Task<Folder> GetFolderByIdAsync(int? folderId);
}