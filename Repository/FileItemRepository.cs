
using DataAccess;
using DataAccess.Entities;

namespace Repository;

public class FileItemRepository(FileManagerDbContext context) : Repository<FileItem>(context), IFileItemRepository;