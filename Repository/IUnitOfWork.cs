// FileManager.DataAccess/Repositories/IUnitOfWork.cs

namespace Repository;

public interface IUnitOfWork : IDisposable
{
    IFileItemRepository Files { get; }
    IFolderRepository Folders { get; }
    Task<int> CompleteAsync();
}