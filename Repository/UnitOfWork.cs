// FileManager.DataAccess/Repositories/UnitOfWork.cs

using DataAccess;

namespace Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly FileManagerDbContext _context;

    public UnitOfWork(FileManagerDbContext context)
    {
        _context = context;
        Files = new FileItemRepository(_context);
        Folders = new FolderRepository(_context);
    }

    public IFileItemRepository Files { get; }
    public IFolderRepository Folders { get; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}