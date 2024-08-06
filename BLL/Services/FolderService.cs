using AutoMapper;
using DataAccess;
using DataAccess.Entities;
using Repository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BLL.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FileManagerDbContext _context;
        private readonly string _storagePath;
        private readonly int? _userId;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileItemRepository _fileItemRepository;

        public FolderService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            FileManagerDbContext context,
            IOptions<StorageSettings> storageSettings,
            UserIdProviderService userIdProvider,
            IFolderRepository folderRepository,
            IFileItemRepository fileItemRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _storagePath = storageSettings?.Value.StoragePath ?? throw new ArgumentNullException(nameof(storageSettings));
            _userId = userIdProvider?.GetUserId() ?? throw new ArgumentNullException(nameof(userIdProvider));
            _folderRepository = folderRepository ?? throw new ArgumentNullException(nameof(folderRepository));
            _fileItemRepository = fileItemRepository ?? throw new ArgumentNullException(nameof(fileItemRepository));
        }

        public async Task<FolderModel> GetFolderByIdAsync(int id)
        {
            var folder = await _unitOfWork.Folders.GetByIdAsync(id);

            return _mapper.Map<FolderModel>(folder);
        }

        public async Task<IEnumerable<FolderModel>> GetAllFoldersAsync()
        {
            var folders = await _unitOfWork.Folders.GetAllAsync();
            return _mapper.Map<IEnumerable<FolderModel>>(folders);
        }

        public async Task AddFolderAsync(FolderModel folderModel)
        {


            var folder = _mapper.Map<Folder>(folderModel);
            var parentFolder = await _context.Folders.FindAsync(folderModel.ParentFolderId);
            folder.CreatedBy = (int)_userId;

            if (folderModel.Name != null)
            {
                folder.Path = parentFolder == null
                    ? Path.Combine(_storagePath, folderModel.Name)
                    : Path.Combine(_storagePath, parentFolder.Path ?? string.Empty, folderModel.Name);
            }

            folder.DateCreated = DateTime.UtcNow;
            folder.DateChanged = DateTime.UtcNow;

            if (!Directory.Exists(folder.Path))
            {
                Directory.CreateDirectory(folder.Path);
            }

            await _unitOfWork.Folders.AddAsync(folder);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RenameFolderAsync(int id, string newName)
        {
            var folder = await _unitOfWork.Folders.GetByIdAsync(id);
            folder.CreatedBy = (int)_userId;
            if (folder != null)
            {
                folder.Name = newName;
                folder.DateChanged = DateTime.UtcNow;
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<bool> DeleteFolderAsync(int folderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var folder = await _folderRepository.GetByIdAsync(folderId);
                if (folder == null) return false;

                var folderPath = folder.Path;

                // Get all child files and folders
                var childFiles = await _fileItemRepository.GetFilesByFolderIdAsync(folderId);
                var childFolders = await _folderRepository.GetSubFoldersAsync(folderId);

                // Delete child files from the database and file system
                foreach (var file in childFiles)
                {
                    await _fileItemRepository.DeleteAsync(file.Id);
                    if (File.Exists(file.FilePath))
                    {
                        File.Delete(file.FilePath);
                    }
                }

                // Recursively delete child folders
                foreach (var childFolder in childFolders)
                {
                    await DeleteFolderAsync(childFolder.Id);
                }

                // Delete the folder from the database
                await _folderRepository.DeleteAsync(folder.Id);

                // Delete the folder from the file system
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
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


        public async Task MoveFolderAsync(int folderId, int parentFolderId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var folder = await _unitOfWork.Folders.GetByIdAsync(folderId);
                var parentFolder = await _context.Folders.FindAsync(parentFolderId);
                
                folder.CreatedBy = (int)_userId;

                if (folder == null)
                {
                    throw new InvalidOperationException($"Folder with ID {folderId} does not exist.");
                }

                var oldPath = folder.Path;

                folder.ParentFolderId = parentFolderId;
                folder.DateChanged = DateTime.UtcNow;

                folder.Path = parentFolder != null
                    ? Path.Combine(_storagePath, parentFolder.Path ?? string.Empty, folder.Name)
                    : Path.Combine(_storagePath, folder.Name);

                await _unitOfWork.CompleteAsync();

                if (Directory.Exists(oldPath))
                {
                    Directory.Move(oldPath, folder.Path);
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
