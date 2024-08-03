using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ViewRepository : IViewRepository
    {
        private readonly FileManagerDbContext _context;

        public ViewRepository(FileManagerDbContext context)
        {
            _context = context;
        }

        // Existing methods...

        public async Task<FileVersion> AddFileVersionAsync(FileVersion fileVersion)
        {
            _context.FileVersions.Add(fileVersion);
            await _context.SaveChangesAsync();
            return fileVersion;
        }

        public async Task<FileVersion> GetLatestFileVersionAsync(int fileItemId)
        {
            return await _context.FileVersions
                .Where(fv => fv.FileItemId == fileItemId)
                .OrderByDescending(fv => fv.VersionDate)
                .FirstOrDefaultAsync();
        }

        public Task<FileVersion> GetByIdAsync(int fileItemId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FileVersion>> GetAllVersionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<FileVersion>> GetLatestVersionByFileItemIdAsync(int fileItemId)
        {
            throw new NotImplementedException();
        }
    }

   
}
