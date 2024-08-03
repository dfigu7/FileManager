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

    public ViewService(IFileItemRepository fileItemRepository, IFolderRepository folderRepository, ILogger<ViewService> logger, IFileVersionRepository fileVersionRepository, IViewRepository viewRepository)
    {
        _fileItemRepository = fileItemRepository;
        _folderRepository = folderRepository;
        _logger = logger;
        _fileVersionRepository = fileVersionRepository;
        _viewRepository = viewRepository;
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
        var file = await _fileItemRepository.GetByIdAsync(fileId);
        if (file == null) return false;

        // Create a file version before renaming
        var fileVersion = new FileVersion
        {
            FileItemId = file.Id,
            Name = file.Name,
            FilePath = file.FilePath,
            FolderId = file.FolderId,
            DateCreated = file.DateCreated,
            DateChanged = file.DateChanged,
            ContentType = file.ContentType,
            CreatedBy = file.CreatedBy,

            Size = file.Size,
            VersionDate = DateTime.UtcNow
        };

        await _fileVersionRepository.AddAsync(fileVersion);

        // Generate the new path
        var newPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file.FilePath)!, newName);

        // Update file details
        file.Name = newName;
        file.FilePath = newPath;

        await _fileItemRepository.UpdateAsync(file);
        return true;
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
        var folder = await _folderRepository.GetByIdAsync(folderId);
        if (folder == null) return false;

        var parentPath = System.IO.Path.GetDirectoryName(folder.Path);
        var newFolderPath = System.IO.Path.Combine(parentPath!, newName);

        // Update folder path and name
        folder.Name = newName;
        folder.Path = newFolderPath;

        // Get all child files and folders
        var childFiles = await _fileItemRepository.GetFilesByFolderIdAsync(folderId);
        var childFolders = await _folderRepository.GetSubFoldersAsync(folderId);

        // Update paths for child files
        foreach (var file in childFiles)
        {
            var relativePath = file.FilePath[folder.Path.Length..];
            file.FilePath = System.IO.Path.Combine(newFolderPath, relativePath);
            await _fileItemRepository.UpdateAsync(file);
        }

        // Update paths for child folders
        foreach (var childFolder in childFolders)
        {
            if (childFolder.Path != null)
            {
                var relativePath = childFolder.Path[folder.Path.Length..];
                childFolder.Path = System.IO.Path.Combine(newFolderPath, relativePath);
            }

            await _folderRepository.UpdateAsync(childFolder);
        }

        await _folderRepository.UpdateAsync(folder);
        return true;
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
