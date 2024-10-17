using DataAccess;
using DataAccess.DTO;
using DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Repository;

namespace BLL.Services;

public class ViewService : IViewService
{
    private readonly IFileItemRepository _fileItemRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly ILogger<ViewService> _logger;
    private readonly IFileVersionRepository _fileVersionRepository;
    private readonly IViewRepository _viewRepository;
    private readonly FileManagerDbContext _context;

    public ViewService(IFileItemRepository fileItemRepository, IFolderRepository folderRepository, ILogger<ViewService> logger, IFileVersionRepository fileVersionRepository, IViewRepository viewRepository, FileManagerDbContext context)
    {
        _fileItemRepository = fileItemRepository;
        _folderRepository = folderRepository;
        _logger = logger;
        _fileVersionRepository = fileVersionRepository;
        _viewRepository = viewRepository;
        _context = context;
    }

    public async Task<IEnumerable<FileItem>> GetFilesInFolderAsync(int folderId)
    {
        string message = $"Fetching files for folder with ID: {folderId}";
        _logger.LogInformation(message);
        var folder = await _folderRepository.GetByIdAsync(folderId);
        if (folder != null) return folder.Files;
        _logger.LogWarning($"Folder with ID {folderId} not found");
        throw new KeyNotFoundException("Folder not found");

    }
    public async Task<IEnumerable<FileItem>> SearchFilesAsync(string name)
    {
        return await _fileItemRepository.SearchByNameAsync(name);
    }

    public async Task<IEnumerable<Folder>> SearchFoldersAsync(string name)
    {
        return await _folderRepository.SearchByNameAsync(name);
    }

    public async Task<bool> RenameFileAsync(int fileId, string newName)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var file = await _fileItemRepository.GetByIdAsync(fileId);
            if (file == null) return false;

            var oldFilePath = file.FilePath;

            // Creates a file version before renaming
            var fileVersion = new FileVersion
            {
                FileItemId = file.Id,
                Name = file.Name,
                FilePath = file.FilePath,
                FolderId = file.FolderId,
                DateCreated = file.DateCreated.ToUniversalTime(),
                DateChanged = file.DateChanged.ToUniversalTime(),
                ContentType = file.ContentType,
                CreatedBy = file.CreatedBy,
                Size = file.Size,
                VersionDate = DateTime.UtcNow
            };

            await _fileVersionRepository.AddAsync(fileVersion);

            // Generates the new path
            var parentFolderPath = System.IO.Path.GetDirectoryName(file.FilePath);
            if (parentFolderPath == null)
                throw new InvalidOperationException("Unable to determine the parent folder path.");

            var newFilePath = System.IO.Path.Combine(parentFolderPath, newName);


            // Updates file details
            file.Name = newName;
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
                // If the file does not exist, rollback the transaction and return false
                await transaction.RollbackAsync();
                return false;
            }

           
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Rollback the transaction if any error occurs
            await transaction.RollbackAsync();
            
            throw new InvalidOperationException("An error occurred while renaming the file.", ex);
        }
    }



    public async Task RollbackFileAsync(int fileItemId)
    {
        var latestVersion = await _fileVersionRepository.GetLatestVersionByFileItemIdAsync(fileItemId);

        if (latestVersion != null)
        {
            var fileItem = await _fileItemRepository.GetByIdAsync(fileItemId);

            if (fileItem != null)
            {
                fileItem.Name = latestVersion.Name;
                fileItem.FilePath = latestVersion.FilePath;
                await _fileItemRepository.UpdateAsync(fileItem);
            }
        }
    }




    public async Task<bool> RenameFolderAsync(int folderId, string newName)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var folder = await _folderRepository.GetByIdAsync(folderId);
            if (folder == null) return false;

            var parentPath = Path.GetDirectoryName(folder.Path);
            var newFolderPath = Path.Combine(parentPath!, newName);

            // Update folder path and name
            var oldFolderPath = folder.Path;
            folder.Name = newName;
            folder.Path = newFolderPath;

            await _viewRepository.UpdateFolderAsync(folder);

            // Get all child files and folders
            var childFiles = await _fileItemRepository.GetFilesByFolderIdAsync(folderId);
            var childFolders = await _folderRepository.GetSubFoldersAsync(folderId);

            // Update paths for child files
            foreach (var file in childFiles)
            {
                var relativePath = file.FilePath[oldFolderPath.Length..];
                file.FilePath = Path.Combine(newFolderPath, relativePath);
                await _fileItemRepository.UpdateAsync(file);
            }

            // Update paths for child folders
            foreach (var childFolder in childFolders)
            {
                var relativePath = childFolder.Path[oldFolderPath.Length..];
                childFolder.Path = Path.Combine(newFolderPath, relativePath);
                await _folderRepository.UpdateAsync(childFolder);
            }

            // Rename folder in the local file system
            Directory.Move(oldFolderPath, newFolderPath);

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }





    public async Task<IEnumerable<FileItem>> GetFilesByFilterAsync(int folderId, FilterSortDto filterSortDto)
    {
        var files = await _fileItemRepository.GetFilesByFolderIdAsync(folderId);
        var query = files.AsQueryable();

        if (!string.IsNullOrEmpty(filterSortDto.FileType))
        {
            query = query.Where(f => f.ContentType == filterSortDto.FileType);
        }

        if (filterSortDto.DateCreatedFrom.HasValue)
        {
            query = query.Where(f => f.DateCreated >= filterSortDto.DateCreatedFrom.Value);
        }

        if (filterSortDto.DateCreatedTo.HasValue)
        {
            query = query.Where(f => f.DateCreated <= filterSortDto.DateCreatedTo.Value);
        }

        query = filterSortDto.SortBy switch
        {
            "name" => filterSortDto.Ascending ? query.OrderBy(f => f.Name) : query.OrderByDescending(f => f.Name),
            "fileType" => filterSortDto.Ascending ? query.OrderBy(f => f.ContentType) : query.OrderByDescending(f => f.ContentType),
            "dateCreated" => filterSortDto.Ascending ? query.OrderBy(f => f.DateCreated) : query.OrderByDescending(f => f.DateCreated),
            _ => query
        };

        return query.ToList();
    }
}
