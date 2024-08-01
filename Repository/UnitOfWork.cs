// FileManager.DataAccess/Repositories/UnitOfWork.cs

using DataAccess;
using Microsoft.AspNetCore.Http;

namespace Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly FileManagerDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWork(FileManagerDbContext context,IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        
        Files = new FileItemRepository(_context);
        Folders = new FolderRepository(_context, _httpContextAccessor);

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