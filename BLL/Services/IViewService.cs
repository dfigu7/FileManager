using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace BLL.Services
{
    public interface IViewService
    {
        Task<IEnumerable<FileItem>> GetFilesInFolderAsync(int folderId);
        Task<IEnumerable<FileItem>> SearchFilesAsync(string name);
        Task<IEnumerable<Folder>> SearchFoldersAsync(string name);
        Task<bool> RenameFileAsync(int fileId, string newName);
        Task<bool> RenameFolderAsync(int folderId, string newName);
        
    }
}
