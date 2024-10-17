// FileManager.DataAccess/Repositories/IUnitOfWork.cs

namespace Repository;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IFileItemRepository Files { get; }
    IFolderRepository Folders { get; }
    Task<int> CompleteAsync();
}