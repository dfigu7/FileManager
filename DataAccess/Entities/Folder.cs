using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json.Serialization;

namespace DataAccess.Entities;

public class Folder
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public int? ParentFolderId { get; set; }
    public int CreatedBy { get; set; }

    public Folder? ParentFolder { get; set; }
    public ICollection<Folder>? SubFolders { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateChanged { get; set; } = DateTime.UtcNow;
 
    public ICollection<FileItem> Files { get; set; } = [];
    public string? Path { get; set; }
}

  