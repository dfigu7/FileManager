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
    public class FileVersionRepository : IFileVersionRepository
    {
        private readonly FileManagerDbContext _context;

        public FileVersionRepository(FileManagerDbContext context)
        {
            _context = context;
        }

        public async Task<FileVersion> AddAsync(FileVersion fileVersion)
        {
            _context.FileVersions.Add(fileVersion);
            await _context.SaveChangesAsync();
            return fileVersion;
        }

        public async Task<IEnumerable<FileVersion>> GetVersionsByFileItemIdAsync(int fileItemId)
        {
            return await _context.FileVersions
                .Where(fv => fv.FileItemId == fileItemId)
                .ToListAsync();
        }

        public async Task<FileVersion> GetLatestVersionByFileItemIdAsync(int fileItemId)
        {
            return await _context.FileVersions
                .Where(fv => fv.FileItemId == fileItemId)
                .OrderByDescending(fv => fv.VersionDate)
                .FirstOrDefaultAsync();
        }
    }

}
