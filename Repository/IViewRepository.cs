using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IViewRepository
    {
        // Existing methods...

        Task<FileVersion> AddFileVersionAsync(FileVersion fileVersion);
        Task<FileVersion> GetLatestFileVersionAsync(int fileItemId);
        Task<FileVersion> GetByIdAsync(int fileItemId);
        Task<List<FileVersion>> GetAllVersionsAsync();
        Task<List<FileVersion>> GetLatestVersionByFileItemIdAsync(int fileItemId);
        Task UpdateFolderAsync(Folder folder);
    }

}
