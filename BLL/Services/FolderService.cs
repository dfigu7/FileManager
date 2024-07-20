using AutoMapper;
using BLL.DTO;
using DataAccess.Entities;
using Repository;

namespace BLL.Services
{
    public class FolderService(IUnitOfWork unitOfWork, IMapper mapper) : IFolderService
    {
        public async Task<FolderModel> GetFolderByIdAsync(int id)
        {
            var folder = await unitOfWork.Folders.GetByIdAsync(id);
            return mapper.Map<FolderModel>(folder);
        }

        public async Task<IEnumerable<FolderModel>> GetAllFoldersAsync()
        {
            var folders = await unitOfWork.Folders.GetAllAsync();
            return mapper.Map<IEnumerable<FolderModel>>(folders);
        }

        public async Task AddFolderAsync(FolderModel folderModel)
        {
            var folder = mapper.Map<Folder>(folderModel);
            folder.DateCreated = DateTime.UtcNow;
            folder.DateChanged = DateTime.UtcNow;
            await unitOfWork.Folders.AddAsync(folder);
            await unitOfWork.CompleteAsync();
        }

        public async Task RenameFolderAsync(int id, string newName)
        {
            var folder = await unitOfWork.Folders.GetByIdAsync(id);
            if (folder != null)
            {
                folder.Name = newName;
                folder.DateChanged = DateTime.UtcNow;
                await unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteFolderAsync(int id)
        {
            var folder = await unitOfWork.Folders.GetByIdAsync(id);
            if (folder != null)
            {
                unitOfWork.Folders.Remove(folder);
                await unitOfWork.CompleteAsync();
            }
        }

        public async Task MoveFolderAsync(int folderId, int parentFolderId)
        {
            var folder = await unitOfWork.Folders.GetByIdAsync(folderId);
            if (folder != null)
            {
                folder.ParentFolderId = parentFolderId;
                folder.DateChanged = DateTime.UtcNow;
                await unitOfWork.CompleteAsync();
            }
        }
    }
}
