// FileManager.BLL/Models/FolderModel.cs

namespace DataAccess.DTO;

public class FolderModel
{
    public required string? Name { get; set; }
    public int? ParentFolderId { get; set; }
    
}