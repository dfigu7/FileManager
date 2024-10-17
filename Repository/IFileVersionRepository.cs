using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFileVersionRepository
    {
        Task<FileVersion> AddAsync(FileVersion fileVersion);
        Task<IEnumerable<FileVersion>> GetVersionsByFileItemIdAsync(int fileItemId);
        Task<FileVersion> GetLatestVersionByFileItemIdAsync(int fileItemId);
    }
}
