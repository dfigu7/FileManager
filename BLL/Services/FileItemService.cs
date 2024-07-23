using System.Diagnostics;
using System.Net;
using AutoMapper;
using DataAccess.Entities;
using BLL.DTO;


using Repository;


namespace BLL.Services
{
    public class FileItemService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
 
        private readonly IRepository<Folder> _folderRepository;

      
        public FileItemService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Folder> folderRepository)
        {
            unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._folderRepository = folderRepository;
            _folderRepository = folderRepository ?? throw new ArgumentNullException(nameof(folderRepository));
        }

      
        public FileItemService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Folder> folderRepository, SomeOtherDependency otherDependency)
            : this(unitOfWork, mapper, folderRepository) 
        {
          
        }

        public Task<FileModel?> GetFileByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileModel>> GetAllFilesAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddFileAsync(FileModel file)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFileAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task MoveFileAsync(int fileId, int folderId)
        {
            throw new NotImplementedException();
        }
    }

    public class SomeOtherDependency
    {
    }

    public interface IFormFile
    {
        string FileName { get; }
        Task CopyToAsync(Stream stream);
        

    }
}
