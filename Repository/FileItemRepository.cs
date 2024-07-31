using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using System.IO;

namespace Repository
{
    public class FileItemRepository : IFileItemRepository
    {
        private readonly FileManagerDbContext _context;

        public FileItemRepository(FileManagerDbContext context)
        {
            _context = context;
        }
        public async Task<FileItem> GetByIdAsync(int id)
        {
            return await _context.FileItems.FindAsync(id);
        }

        public async Task UpdateAsync(FileItem file)
        {
            _context.FileItems.Update(file);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetFullPath(int folderId)
        {
            var folder = await _context.Folders.FindAsync(folderId);
            return folder?.Path ?? string.Empty;
        }



        public async Task<IEnumerable<FileItem>> GetAllAsync()
        {
            return await _context.FileItems.ToListAsync();
        }
        public async Task AddFileItemAsync(FileItem fileItem)
        {
            _context.FileItems.Add(fileItem);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> FileExistsAsync(string name, int folderId)
        {
            return await _context.FileItems
                .AnyAsync(f => f.Name == name && f.FolderId == folderId);
        }
        public async Task<FileItem> GetFileItemByNameAsync(string fileName)
        {
            return await _context.FileItems.FirstOrDefaultAsync(f => f.Name == fileName) ?? throw new InvalidOperationException();
        }
        public async Task<IEnumerable<FileItem>> SearchByNameAsync(string name)
        {
            return await _context.FileItems
                .Where(fi => fi.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<FileItem> AddAsync(FileItem fileItem)
        {
            _context.FileItems.Add(fileItem);
            await _context.SaveChangesAsync();
            return fileItem;
        }

        public async Task DeleteAsync(int id)
        {
            var fileItem = await _context.FileItems.FindAsync(id);
            if (fileItem != null)
            {
                _context.FileItems.Remove(fileItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MoveAsync(int fileId, int folderId)
        {
            var fileItem = await _context.FileItems.FindAsync(fileId);
            if (fileItem != null)
            {
                fileItem.FolderId = folderId;
                await _context.SaveChangesAsync();
            }
        }
    }
}