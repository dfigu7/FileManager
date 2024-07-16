// FileManager.BLL/Services/FolderService.cs

using AutoMapper;
using BLL.Models;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BLL.Services;

public class FolderService : IFolderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FolderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        await _unitOfWork.Folders.AddAsync(folder);
        await _unitOfWork.CompleteAsync();
    }

    public async Task RenameFolderAsync(int id, string newName)
    {
        var folder = await _unitOfWork.Folders.GetByIdAsync(id);
        if (folder != null)
        {
            folder.Name = newName;
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task DeleteFolderAsync(int id)
    {
        var folder = await _unitOfWork.Folders.GetByIdAsync(id);
        if (folder != null)
        {
            _unitOfWork.Folders.Remove(folder);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task MoveFolderAsync(int folderId, int parentFolderId)
    {
        var folder = await _unitOfWork.Folders.GetByIdAsync(folderId);
        if (folder != null)
        {
            folder.ParentFolderId = parentFolderId;
            await _unitOfWork.CompleteAsync();
        }
    }
}