// FileManager.DataAccess/Repositories/IUnitOfWork.cs

namespace DataAccess.Repositories;

public interface IUnitOfWork : IDisposable
{
    IFileRepository Files { get; }
    IFolderRepository Folders { get; }
    Task<int> CompleteAsync();
}