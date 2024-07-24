using BLL;
using BLL.DTO;
using BLL.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly StorageSettings _storageSettings;

        public FilesController(IFileItemService fileItemService, IOptions<StorageSettings> storageSettings)
        {
            _fileItemService = fileItemService;
            _storageSettings = storageSettings.Value;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadPath = _storageSettings.StoragePath;

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Save file metadata to the database
            var fileItem = new FileItem
            {
                Name = file.FileName,
                FilePath = filePath,
                Size = file.Length,
                ContentType = file.ContentType
            };

            await _fileItemService.AddFileItemAsync(fileItem);

            return Ok(new { FilePath = filePath });
        }


        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var fileItem = await _fileItemService.GetFileItemByNameAsync(fileName);

            if (fileItem == null)
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(fileItem.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetContentType(fileItem.FilePath), fileItem.Name);
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".csv", "text/csv" }
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FileModel>> GetFileById(int id)
        {
            var file = await _fileItemService.GetByIdAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            return Ok(file);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetAllFiles()
        {
            var files = await _fileItemService.GetAllAsync();
            return Ok(files);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile([FromBody] FileModel fileModel)
        {
            await _fileItemService.AddFileAsync(fileModel);
            return CreatedAtAction(nameof(GetFileById), new { id = fileModel.Id }, fileModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            await _fileItemService.DeleteFileAsync(id);
            return NoContent();
        }

        [HttpPut("{fileId}/move/{folderId}")]
        public async Task<IActionResult> MoveFile(int fileId, int folderId)
        {
            await _fileItemService.MoveFileAsync(fileId, folderId);
            return NoContent();
        }
    }
}