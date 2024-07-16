// FileManager.DataAccess/Repositories/FolderRepository.cs

using DataAccess.Entities;
using Repositories;

namespace DataAccess.Repositories;

public class FolderRepository : Repository<Folder>, IFolderRepository
{
    public FolderRepository(FileManagerDbContext context) : base(context)
    {
    }
}