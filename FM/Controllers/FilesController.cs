// FileManager/Controllers/FilesController.cs

using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController(IFileService fileService) : ControllerBase
{
    private readonly IFileService _fileService = fileService;

    [HttpGet("{id}")]
    public async Task<ActionResult<FileModel>> GetFileById(int id)
    {
        var file = await _fileService.GetFileByIdAsync(id);
        if (file == null) return NotFound();
        return Ok(file);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileModel>>> GetAllFiles()
    {
        var files = await _fileService.GetAllFilesAsync();
        return Ok(files);
    }

    [HttpPost]
    public async Task<ActionResult> AddFile([FromBody] FileModel fileModel)
    {
        await _fileService.AddFileAsync(fileModel);
        return Ok();
        //return CreatedAtAction(nameof(GetFileById), new { id = fileModel.Id }, fileModel);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFile(int id)
    {
        await _fileService.DeleteFileAsync(id);
        return NoContent();
    }

    [HttpPut("move")]
    public async Task<ActionResult> MoveFile(int fileId, int folderId)
    {
        await _fileService.MoveFileAsync(fileId, folderId);
        return NoContent();
    }
}