using AutoMapper;
using BLL.DTO;
using DataAccess.Entities;
using Repository;

namespace BLL.Services
{
    public class FileService(IUnitOfWork unitOfWork, IMapper mapper) : IFileService
    {
        public async Task<FileModel?> GetFileByIdAsync(int id)
        {
            var file = await unitOfWork.Files.GetByIdAsync(id);
            return mapper.Map<FileModel>(file);
        }

        public async Task<IEnumerable<FileModel>> GetAllFilesAsync()
        {
            var files = await unitOfWork.Files.GetAllAsync();
            return mapper.Map<IEnumerable<FileModel>>(files);
        }

        public async Task AddFileAsync(FileModel fileModel)
        {
            var file = mapper.Map<FileItem>(fileModel);
            file.DateCreated = DateTime.UtcNow;
            file.DateChanged = DateTime.UtcNow;
            await unitOfWork.Files.AddAsync(file);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteFileAsync(int id)
        {
            var file = await unitOfWork.Files.GetByIdAsync(id);
            if (file != null)
            {
                unitOfWork.Files.Remove(file);
                await unitOfWork.CompleteAsync();
            }
        }

        public async Task MoveFileAsync(int fileId, int folderId)
        {
            var file = await unitOfWork.Files.GetByIdAsync(fileId);
            if (file != null)
            {
                file.FolderId = folderId;
                file.DateChanged = DateTime.UtcNow;
                await unitOfWork.CompleteAsync();
            }
        }
    }
}