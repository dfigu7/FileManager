using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class FolderRepository : Repository<Folder>, IFolderRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly FileManagerDbContext _context;
    private readonly IFileItemRepository _fileItemRepository;
   

    public FolderRepository(FileManagerDbContext context,IHttpContextAccessor httpContextAccessor  ) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        
    }

    // Ensure eager loading of Files
    public new async Task<Folder> GetByIdAsync(int id)
    {
        return await _context.Folders
            .Include(f => f!.Files)
            .FirstOrDefaultAsync(f => f != null && f.Id == id) ?? throw new InvalidOperationException();

    }
    public async Task<IEnumerable<Folder>> SearchByNameAsync(string name)
    {
        return (await _context.Folders
            .Where(f => f.Name.Contains(name))
            .ToListAsync())!;
    }
    

    public async Task UpdateAsync(Folder folder)
    {
        _context.Folders.Update(folder);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int folderId)
    {
        var folder = await _context.Folders.FindAsync(folderId);
        if (folder == null)
        {
            return false;
        }

        _context.Folders.Remove(folder);

        // Optionally, remove associated files and subfolders
        var childFiles = _context.FileItems.Where(f => f.FolderId == folderId);
        var childFolders = _context.Folders.Where(f => f.ParentFolderId == folderId);

        _context.FileItems.RemoveRange(childFiles);
        _context.Folders.RemoveRange(childFolders);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Folder>> GetSubFoldersAsync(int parentId)
    {

        return await _context.Folders.Where(f => f.ParentFolderId == parentId).ToListAsync();
    }
    public async Task<Folder> GetFolderByIdAsync(int? folderId)
    {
        return await _context.Folders.FindAsync(folderId);
    }
}