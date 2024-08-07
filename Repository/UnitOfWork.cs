// FileManager.DataAccess/Repositories/UnitOfWork.cs

using AutoMapper;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly FileManagerDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UnitOfWork(FileManagerDbContext context,IHttpContextAccessor httpContextAccessor, IMapper _mapper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        
        
        Files = new FileItemRepository(_context);
        Folders = new FolderRepository(_context, _httpContextAccessor);
        Users = new UserRepository(_context, _mapper);

    }

    public IUserRepository Users { get; private set; }
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