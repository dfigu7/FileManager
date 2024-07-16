// FileManager.DataAccess/Repositories/FolderRepository.cs

using DataAccess.Entities;

namespace DataAccess.Repositories;

public class FolderRepository : Repository<Folder>, IFolderRepository
{
    public FolderRepository(FileManagerDbContext context) : base(context)
    {
    }
}