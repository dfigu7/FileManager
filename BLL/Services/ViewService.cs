using DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Repository;

namespace BLL.Services;

public class ViewService : IViewService
{
    private readonly IFileItemRepository _fileItemRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly ILogger<ViewService> _logger;

    public ViewService(IFileItemRepository fileItemRepository, IFolderRepository folderRepository, ILogger<ViewService> logger)
    {
        _fileItemRepository = fileItemRepository;
        _folderRepository = folderRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<FileItem>> GetFilesInFolderAsync(int folderId)
    {
        _logger.LogInformation($"Fetching files for folder with ID: {folderId}");
        var folder = await _folderRepository.GetByIdAsync(folderId);
        if (folder == null)
        {
            _logger.LogWarning($"Folder with ID {folderId} not found");
            throw new KeyNotFoundException("Folder not found");
        }

        return folder.Files;
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

        var newPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file.FilePath), newName);
        file.Name = newName;
        file.FilePath = newPath;

        await _fileItemRepository.UpdateAsync(file);
        return true;
    }

    public async Task<bool> RenameFolderAsync(int folderId, string newName)
    {
        var folder = await _folderRepository.GetByIdAsync(folderId);
        if (folder == null) return false;

        var parentPath = System.IO.Path.GetDirectoryName(folder.Path);
        var newFolderPath = System.IO.Path.Combine(parentPath, newName);

        // Update folder path and name
        folder.Name = newName;
        folder.Path = newFolderPath;

        // Get all child files and folders
        var childFiles = await _fileItemRepository.GetFilesByFolderIdAsync(folderId);
        var childFolders = await _folderRepository.GetSubFoldersAsync(folderId);

        // Update paths for child files
        foreach (var file in childFiles)
        {
            var relativePath = file.FilePath.Substring(folder.Path.Length);
            file.FilePath = System.IO.Path.Combine(newFolderPath, relativePath);
            await _fileItemRepository.UpdateAsync(file);
        }

        // Update paths for child folders
        foreach (var childFolder in childFolders)
        {
            var relativePath = childFolder.Path.Substring(folder.Path.Length);
            childFolder.Path = System.IO.Path.Combine(newFolderPath, relativePath);
            await _folderRepository.UpdateAsync(childFolder);
        }

        await _folderRepository.UpdateAsync(folder);
        return true;
    }
}
