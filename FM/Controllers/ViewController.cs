﻿using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace FMAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ViewController : ControllerBase
{
    private readonly IViewService _viewService;
    private readonly ILogger<ViewController> _logger;

    public ViewController(IViewService viewService, ILogger<ViewController> logger)
    {
        _viewService = viewService;
        _logger = logger;
    }

    [HttpGet("{folderId}/files")]
    public async Task<IActionResult> GetFilesInFolder(int folderId)
    {
        try
        {
            _logger.LogInformation($"Request to get files in folder ID: {folderId}");
            var files = await _viewService.GetFilesInFolderAsync(folderId);
            return Ok(files);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning($"Folder ID: {folderId} not found");
            return NotFound("Folder not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching files for folder ID: {folderId}");
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string name)
    {
        var files = await _viewService.SearchFilesAsync(name);
        var folders = await _viewService.SearchFoldersAsync(name);

        var result = new
        {
            Files = files,
            Folders = folders
        };

        return Ok(result);
    }
}