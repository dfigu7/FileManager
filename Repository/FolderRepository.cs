using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class FolderRepository : Repository<Folder>, IFolderRepository
{
    public FolderRepository(FileManagerDbContext context) : base(context)
    {
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

    public async Task<IEnumerable<Folder>> GetSubFoldersAsync(int parentId)
    {
        return await _context.Folders.Where(f => f.ParentFolderId == parentId).ToListAsync();
    }
}