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
}