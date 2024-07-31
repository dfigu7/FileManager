using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using System.IO;

namespace Repository
{
    public class FileItemRepository(FileManagerDbContext context) : IFileItemRepository
    {
        private IFileItemRepository _fileItemRepositoryImplementation;

        public async Task<FileItem> GetByIdAsync(int id)
        {
            return await context.FileItems.FindAsync(id) ?? throw new InvalidOperationException();
        }

        public async Task UpdateAsync(FileItem file)
        {
            context.FileItems.Update(file);
            await context.SaveChangesAsync();
        }

        public async Task<string> GetFullPath(int folderId)
        {
            var folder = await context.Folders.FindAsync(folderId);
            return folder?.Path ?? string.Empty;
        }
       

     

        public async Task<IEnumerable<FileItem>> GetFilesByFolderIdAsync(int folderId)
        {
            return await context.FileItems.Where(fi => fi.FolderId == folderId).ToListAsync();
        }

        public object GetFilesByFolderId(int folderId)
        {
            return _fileItemRepositoryImplementation.GetFilesByFolderId(folderId);
        }


        public async Task<IEnumerable<FileItem>> GetAllAsync()
        {
            return await context.FileItems.ToListAsync();
        }
        public async Task AddFileItemAsync(FileItem fileItem)
        {
            context.FileItems.Add(fileItem);
            await context.SaveChangesAsync();
        }
        public async Task<bool> FileExistsAsync(string name, int folderId)
        {
            return await context.FileItems
                .AnyAsync(f => f.Name == name && f.FolderId == folderId);
        }
        public async Task<FileItem> GetFileItemByNameAsync(string fileName)
        {
            return await context.FileItems.FirstOrDefaultAsync(f => f.Name == fileName) ?? throw new InvalidOperationException();
        }
        public async Task<IEnumerable<FileItem>> SearchByNameAsync(string name)
        {
            return await context.FileItems
                .Where(fi => fi.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<FileItem> AddAsync(FileItem fileItem)
        {
            context.FileItems.Add(fileItem);
            await context.SaveChangesAsync();
            return fileItem;
        }

        public async Task DeleteAsync(int id)
        {
            var fileItem = await context.FileItems.FindAsync(id);
            if (fileItem != null)
            {
                context.FileItems.Remove(fileItem);
                await context.SaveChangesAsync();
            }
        }

        public async Task MoveAsync(int fileId, int folderId)
        {
            var fileItem = await context.FileItems.FindAsync(fileId);
            if (fileItem != null)
            {
                fileItem.FolderId = folderId;
                await context.SaveChangesAsync();
            }
        }
    }
}