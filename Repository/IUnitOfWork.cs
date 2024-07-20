// FileManager.DataAccess/Repositories/IUnitOfWork.cs

using DataAccess.Repositories;

namespace Repository;

public interface IUnitOfWork : IDisposable
{
    IFileRepository Files { get; }
    IFolderRepository Folders { get; }
    Task<int> CompleteAsync();
}