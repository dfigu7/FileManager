using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoldersController(IFolderService folderService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<FolderModel>> GetFolderById(int id)
    {
        var folder = await folderService.GetFolderByIdAsync(id);
        if (folder == null) return NotFound();
        return Ok(folder);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FolderModel>>> GetAllFolders()
    {
        var folders = await folderService.GetAllFoldersAsync();
        return Ok(folders);
    }

    [HttpPost]
    public async Task<ActionResult> AddFolder([FromBody] FolderModel folderModel)
    {
        await folderService.AddFolderAsync(folderModel);

        return Ok();
       
    }

    [HttpPut("{id}/rename")]
    public async Task<ActionResult> RenameFolder(int id, [FromBody] string newName)
    {
        await folderService.RenameFolderAsync(id, newName);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFolder(int id)
    {
        await folderService.DeleteFolderAsync(id);
        return NoContent();
    }

    [HttpPut("move")]
    public async Task<ActionResult> MoveFolder(int folderId, int parentFolderId)
    {
        await folderService.MoveFolderAsync(folderId, parentFolderId);
        return NoContent();
    }
}