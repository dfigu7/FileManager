using DataAccess.DTO;

namespace BLL.Services;

public interface IFolderService
{
    Task<FolderModel> GetFolderByIdAsync(int id);
    Task<IEnumerable<FolderModel>> GetAllFoldersAsync();
    Task AddFolderAsync(FolderModel folder);
    Task RenameFolderAsync(int id, string newName);
    Task<bool> DeleteFolderAsync(int id);
    Task MoveFolderAsync(int folderId, int parentFolderId);

}