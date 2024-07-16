// FileManager.BLL/Services/FileService.cs

using AutoMapper;
using BLL.Models;
using DataAccess.Entities;
using DataAccess.Repositories;
using Repositories;

namespace BLL.Services;

public class FileService : IFileService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FileService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FileModel> GetFileByIdAsync(int id)
    {
        var file = await _unitOfWork.Files.GetByIdAsync(id);
        return _mapper.Map<FileModel>(file);
    }

    public async Task<IEnumerable<FileModel>> GetAllFilesAsync()
    {
        var files = await _unitOfWork.Files.GetAllAsync();
        return _mapper.Map<IEnumerable<FileModel>>(files);
    }

    public async Task AddFileAsync(FileModel fileModel)
    {
        var file = _mapper.Map<FileItem>(fileModel);
        await _unitOfWork.Files.AddAsync(file);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteFileAsync(int id)
    {
        var file = await _unitOfWork.Files.GetByIdAsync(id);
        if (file != null)
        {
            _unitOfWork.Files.Remove(file);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task MoveFileAsync(int fileId, int folderId)
    {
        var file = await _unitOfWork.Files.GetByIdAsync(fileId);
        if (file != null)
        {
            file.FolderId = folderId;
            await _unitOfWork.CompleteAsync();
        }
    }
}