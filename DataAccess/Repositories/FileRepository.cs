// FileManager.DataAccess/Repositories/FileRepository.cs

namespace DataAccess.Repositories;

public class FileRepository(FileManagerDbContext context) : Repository<File>(context), IFileRepository;