
using BLL.Services;
using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FMAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoldersController(IFolderService folderService) : ControllerBase
{
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<FolderModel>> GetFolderById(int id)
    {
        var folder = await folderService.GetFolderByIdAsync(id);
        if (folder == null) return NotFound();
        return Ok(folder);
    }

    [HttpGet,Authorize]
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




    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFolder(int id)
    {
        await folderService.DeleteFolderAsync(id);
        return NoContent();
    }

    [HttpPut("moveFolder")]
    public async Task<IActionResult> MoveFolder(int folderId, int targetFolderId)
    {
        try
        {
            await folderService.MoveFolderAsync(folderId, targetFolderId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}


