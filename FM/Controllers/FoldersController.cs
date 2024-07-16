using Microsoft.AspNetCore.Mvc;

namespace FMAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoldersController(IFolderService folderService) : ControllerBase
{
    private readonly IFolderService _folderService = folderService;

    [HttpGet("{id}")]
    public async Task<ActionResult<FolderModel>> GetFolderById(int id)
    {
        var folder = await _folderService.GetFolderByIdAsync(id);
        if (folder == null) return NotFound();
        return Ok(folder);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FolderModel>>> GetAllFolders()
    {
        var folders = await _folderService.GetAllFoldersAsync();
        return Ok(folders);
    }

    [HttpPost]
    public async Task<ActionResult> AddFolder([FromBody] FolderModel folderModel)
    {
        await _folderService.AddFolderAsync(folderModel);
        return CreatedAtAction(nameof(GetFolderById), new { id = folderModel.Id }, folderModel);
    }

    [HttpPut("{id}/rename")]
    public async Task<ActionResult> RenameFolder(int id, [FromBody] string newName)
    {
        await _folderService.RenameFolderAsync(id, newName);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFolder(int id)
    {
        await _folderService.DeleteFolderAsync(id);
        return NoContent();
    }

    [HttpPut("move")]
    public async Task<ActionResult> MoveFolder(int folderId, int parentFolderId)
    {
        await _folderService.MoveFolderAsync(folderId, parentFolderId);
        return NoContent();
    }
}