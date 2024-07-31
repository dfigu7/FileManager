// FileManager.DataAccess/Repositories/IFolderRepository.cs

using DataAccess.Entities;

namespace Repository;

public interface IFolderRepository : IRepository<Folder>
{
    // Additional methods specific to Folder repository
    Task<IEnumerable<Folder>> SearchByNameAsync(string name);
}