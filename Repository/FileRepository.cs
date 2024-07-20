// FileManager.DataAccess/Repositories/FileRepository.cs

using DataAccess.Entities;
using Repositories;
using Repository;

namespace DataAccess.Repositories;

public class FileRepository(FileManagerDbContext context) : Repository<FileItem>(context), IFileRepository;