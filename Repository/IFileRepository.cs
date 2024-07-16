// FileManager.DataAccess/Repositories/IFileRepository.cs

using DataAccess.Entities;

namespace DataAccess.Repositories;

public interface IFileRepository : IRepository<FileItem>
{
    // Additional methods specific to File repository
}