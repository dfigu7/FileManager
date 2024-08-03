using AutoMapper;
using DataAccess.Entities;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class FileItemService : IFileItemService
    {
        private readonly IFileItemRepository _fileItemRepository;
        private readonly IMapper _mapper;
        private readonly FileManagerDbContext _context;
        private readonly int? _userId;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public FileItemService(IFileItemRepository fileItemRepository, IMapper mapper, UserIdProviderService userIdProvider)
        {
            _fileItemRepository = fileItemRepository;
            _mapper = mapper;
            _userId = userIdProvider.GetUserId();
           
        }
        public async Task<bool> FileExistsAsync(string name, int folderId)
        {
            return await _fileItemRepository.FileExistsAsync(name, folderId);
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
            
            file.CreatedBy = (int)_userId;
            await _fileItemRepository.AddAsync(fileItem);
        }

        public async Task DeleteFileAsync(int id)
        {
            await _fileItemRepository.DeleteAsync(id);
        }

        public async Task MoveFileAsync(int fileId, int targetFolderId)
        {
            var file = await _fileItemRepository.GetByIdAsync(fileId) ?? throw new Exception("File not found");
            file.FolderId = targetFolderId;
            file.FilePath = await _fileItemRepository.GetFullPath(targetFolderId) + "/" + file.Name;

            await _fileItemRepository.UpdateAsync(file);
        }

    }

    
}