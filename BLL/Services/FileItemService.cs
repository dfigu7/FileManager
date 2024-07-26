using AutoMapper;
using BLL.DTO;
using DataAccess.Entities;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FileItemService : IFileItemService
    {
        private readonly IFileItemRepository _fileItemRepository;
        private readonly IMapper _mapper;

        public FileItemService(IFileItemRepository fileItemRepository, IMapper mapper)
        {
            _fileItemRepository = fileItemRepository;
            _mapper = mapper;
        }

        public async Task<FileModel> GetByIdAsync(int id)
        {
            var fileItem = await _fileItemRepository.GetByIdAsync(id);
            return _mapper.Map<FileModel>(fileItem);
        }
        public async Task AddFileItemAsync(FileItem fileItem)
        {
            await _fileItemRepository.AddFileItemAsync(fileItem);
        }

        public async Task<FileItem> GetFileItemByNameAsync(string fileName)
        {
            return await _fileItemRepository.GetFileItemByNameAsync(fileName);
        }
        public async Task<IEnumerable<FileModel>> GetAllAsync()
        {
            var fileItems = await _fileItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FileModel>>(fileItems);
        }

        public async Task AddFileAsync(FileItem file)
        {
            var fileItem = _mapper.Map<FileItem>(file);
            await _fileItemRepository.AddAsync(fileItem);
        }

        public async Task DeleteFileAsync(int id)
        {
            await _fileItemRepository.DeleteAsync(id);
        }

        public async Task MoveFileAsync(int fileId, int folderId)
        {
            await _fileItemRepository.MoveAsync(fileId, folderId);
        }
       
    }

    
}