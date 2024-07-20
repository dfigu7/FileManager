// FileManager/Controllers/FilesController.cs

using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(IFileService fileService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<FileModel>> GetFileById(int id)
        {
            var file = await fileService.GetFileByIdAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            return Ok(file);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetAllFiles()
        {
            var files = await fileService.GetAllFilesAsync();
            return Ok(files);
        }

        [HttpPost]
        public async Task<ActionResult> AddFile([FromBody] FileModel fileModel)
        {
            await fileService.AddFileAsync(fileModel);
            return CreatedAtAction(nameof(GetFileById), new { id = fileModel.Id }, fileModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFile(int id)
        {
            await fileService.DeleteFileAsync(id);
            return NoContent();
        }

        [HttpPut("move")]
        public async Task<ActionResult> MoveFile(int fileId, int folderId)
        {
            await fileService.MoveFileAsync(fileId, folderId);
            return NoContent();
        }
    }
}