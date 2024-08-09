using BLL.Services;
using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FoldersController(IFolderService folderService)
        {
            _folderService = folderService ?? throw new ArgumentNullException(nameof(folderService));
        }

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
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFolder(int id)
        {
            await _folderService.DeleteFolderAsync(id);
            return NoContent();
        }

        [HttpPut("moveFolder")]
        public async Task<IActionResult> MoveFolder([FromQuery] int folderId, [FromQuery] int targetFolderId)
        {
            try
            {
                await _folderService.MoveFolderAsync(folderId, targetFolderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("zip/{folderId}")]
        public async Task<IActionResult> ZipFolder(int folderId)
        {
            var result = await _folderService.ZipFolderAsync(folderId);

            if (!result)
            {
                return BadRequest(new { message = "Failed to zip folder." });
            }

            return Ok(new { message = "Folder zipped successfully." });
        }

        [HttpPost("unzip/{folderId}")]
        public async Task<IActionResult> UnzipFolder(int folderId, IFormFile zipFile)
        {
            if (zipFile == null || zipFile.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            var result = await _folderService.UnzipFolderAsync(folderId);

            if (!result)
            {
                return BadRequest(new { message = "Failed to unzip folder." });
            }

            return Ok(new { message = "Folder unzipped successfully." });
        }
    }
}
