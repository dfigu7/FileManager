// FileManager.DataAccess/Repositories/FolderRepository.cs

using DataAccess.Entities;
using Repository;

namespace DataAccess.Repositories;

public class FolderRepository(FileManagerDbContext context) : Repository<Folder>(context), IFolderRepository;