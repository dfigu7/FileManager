using AutoMapper;
using DataAccess;
using DataAccess.Entities;
using Repository;
using Microsoft.Extensions.Options;

using DataAccess.DTO;

using System.IO.Compression;

namespace BLL.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FileManagerDbContext _context;
        private readonly string _storagePath;
        private readonly UserIdProviderService _userIdProvider;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileItemRepository _fileItemRepository;
        private readonly int _userId;
        

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
            _userIdProvider = userIdProvider ?? throw new ArgumentNullException(nameof(userIdProvider));
            _folderRepository = folderRepository ?? throw new ArgumentNullException(nameof(folderRepository));
            _fileItemRepository = fileItemRepository ?? throw new ArgumentNullException(nameof(fileItemRepository));
            _userId = _userIdProvider.GetUserId() ??  throw new ArgumentNullException("User ID not found.");
        
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
            // Start the transaction at the top-level
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Start the recursive deletion process
                var result = await DeleteFolderRecursiveAsync(folderId);

                if (result)
                {
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<bool> DeleteFolderRecursiveAsync(int folderId)
        {
            var folder = await _folderRepository.GetByIdAsync(folderId);
            if (folder == null) return false;

            var folderPath = folder.Path;

            // Recursively delete child folders first
            var childFolders = await _folderRepository.GetSubFoldersAsync(folderId);
            foreach (var childFolder in childFolders)
            {
                var result = await DeleteFolderRecursiveAsync(childFolder.Id);
                if (!result)
                {
                    return false; // Rollback if any child folder deletion fails
                }
            }

            // Delete child files after deleting child folders
            var childFiles = await _fileItemRepository.GetFilesByFolderIdAsync(folderId);
            foreach (var file in childFiles)
            {
                await _fileItemRepository.DeleteAsync(file.Id);
                if (File.Exists(file.FilePath))
                {
                    File.Delete(file.FilePath);
                }
            }

            // Delete the folder from the database
            await _folderRepository.DeleteAsync(folder.Id);

            // Delete the folder from the file system
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

            return true;
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

        public async Task<bool> ZipFolderAsync(int folderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var folder = await _folderRepository.GetByIdAsync(folderId);
                if (folder == null) return false;

                var folderPath = folder.Path;
                var tempZipPath = Path.Combine(Path.GetTempPath(), $"{folderPath}.zip");

                // Zip the folder
                ZipFile.CreateFromDirectory(folderPath, tempZipPath);

                // Save the zip file information to the database (if needed)
                // ...

                // Move the zip file to the desired location
                var finalZipPath = Path.Combine(folderPath, $"{folderPath}.zip");
                File.Move(tempZipPath, finalZipPath);

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UnzipFolderAsync(int folderId)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        var folder = await _folderRepository.GetByIdAsync(folderId);
                        if (folder == null) return false;

                        var folderPath = folder.Path;
                        var zipFilePath = Path.Combine(folderPath, $".zip");
                        var tempExtractPath = Path.Combine(Path.GetTempPath(), $"{folderId}_extracted");

                        // Create temp extraction directory
                        Directory.CreateDirectory(tempExtractPath);

                        // Unzip the folder
                        ZipFile.ExtractToDirectory(zipFilePath, tempExtractPath);

                        // Move the extracted contents to the final location
                        foreach (var file in Directory.GetFiles(tempExtractPath))
                        {
                            var destFile = Path.Combine(folderPath, Path.GetFileName(file));
                            File.Move(file, destFile);
                        }
                        foreach (var subFolder in Directory.GetDirectories(tempExtractPath))
                        {
                            var destFolder = Path.Combine(folderPath, Path.GetFileName(subFolder));
                            Directory.Move(subFolder, destFolder);
                        }

                        // Clean up temporary directory
                        Directory.Delete(tempExtractPath, true);

                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
        public async Task<string> ZipFilesByDateAsync(DateTime date, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (date.Kind == DateTimeKind.Unspecified)
            {
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            }

           
            DateTime effectiveStartDate = startDate ?? date;
            DateTime effectiveEndDate = endDate ?? date;

            // Generates Name for the zip file
            string zipFileName = startDate.HasValue && endDate.HasValue
                ? $"Files_{effectiveStartDate:yyyyMMdd}_to_{effectiveEndDate:yyyyMMdd}.zip"
                : $"Files_{date:yyyyMMdd}.zip";

            var zipFilePath = Path.Combine(Path.GetTempPath(), zipFileName);

           
            if (File.Exists(zipFilePath))
            {
                return zipFilePath;
            }

            // Fetch files by date or by range
            var files = startDate.HasValue && endDate.HasValue
                ? await _fileItemRepository.GetFilesByDateRangeAsync(effectiveStartDate, effectiveEndDate)
                : await _fileItemRepository.GetFilesByDateAsync(date);

            if (files == null || !files.Any()) return null;

            // Creates a temporary folder 
            var tempFolder = Path.Combine(Path.GetTempPath(), $"Files_{DateTime.Now:yyyyMMdd_HHmmss}_Temp");
            Directory.CreateDirectory(tempFolder);

            foreach (var file in files)
            {
                // Fetch the folder structure or folder name based on the file's folder ID
                var folder = await _folderRepository.GetFolderByIdAsync(file.FolderId);
                var folderName = folder?.Name ?? "";

               
                var fileName = Path.GetFileName(file.FilePath); // Extract original file name
                var renamedFileName = $"storage_{folderName}_{fileName}"; // Create the new name

                // Copies the file to the temp folder with the new name
                var destinationPath = Path.Combine(tempFolder, renamedFileName);
                File.Copy(file.FilePath, destinationPath);
            }

           
            ZipFile.CreateFromDirectory(tempFolder, zipFilePath);

           
            Directory.Delete(tempFolder, true);

            return zipFilePath;
        }



    }

}

