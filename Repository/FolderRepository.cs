// FileManager.DataAccess/Repositories/FolderRepository.cs

using DataAccess;
using DataAccess.Entities;

namespace Repository;

public class FolderRepository(FileManagerDbContext context) : Repository<Folder>(context), IFolderRepository;