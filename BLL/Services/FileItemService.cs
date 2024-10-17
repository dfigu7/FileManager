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
        private readonly IFolderRepository _folderRepository;
       

        public FileItemService(IFileItemRepository fileItemRepository, IMapper mapper, UserIdProviderService userIdProvider, IFolderRepository folderRepository, FileManagerDbContext context)
        {
            _fileItemRepository = fileItemRepository;
            _mapper = mapper;
            _userId = userIdProvider.GetUserId();
            _folderRepository = folderRepository;
            _context = context;

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

        public async Task<bool> DeleteFileAsync(int fileId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                var fileItem = await _fileItemRepository.GetByIdAsync(fileId);
                if (fileItem == null) return false;

                
                var filePath = fileItem.FilePath;

               
                await _fileItemRepository.DeleteAsync(fileId);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                
                await transaction.RollbackAsync();
                throw;
            }
        }



        public async Task<bool> MoveFileAsync(int fileId, int folderId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var file = await _fileItemRepository.GetByIdAsync(fileId);
                if (file == null) return false;

                var oldFilePath = file.FilePath;

                // Gets the new folder
                var newFolder = await _folderRepository.GetByIdAsync(folderId);
                if (newFolder == null) throw new InvalidOperationException("The destination folder does not exist.");

                // Generates the new file path
                var newFilePath = System.IO.Path.Combine(newFolder.Path, file.Name);

                // Updates file details
                file.FolderId = folderId;
                file.FilePath = newFilePath;

                // Updates file in the database
                await _fileItemRepository.UpdateAsync(file);

                // Moves the file in the file system
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Move(oldFilePath, newFilePath);
                }
                else
                {
                    throw new FileNotFoundException("The file to move does not exist.");
                }

                
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                
                await transaction.RollbackAsync();
              
                throw new InvalidOperationException("An error occurred while moving the file.", ex);
            }
        }


    }


}