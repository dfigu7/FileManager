﻿using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Repository;

public class FolderRepository : Repository<Folder>, IFolderRepository
{
    public FolderRepository(FileManagerDbContext context) : base(context)
    {
    }

    // Ensure eager loading of Files
    public new async Task<Folder?> GetByIdAsync(int id)
    {
        return await _context.Folders
            .Include(f => f.Files)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
    public async Task<IEnumerable<Folder>> SearchByNameAsync(string name)
    {
        return await _context.Folders
            .Where(f => f.Name.Contains(name))
            .ToListAsync();
    }
}