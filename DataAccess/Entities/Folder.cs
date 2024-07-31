using System.ComponentModel.DataAnnotations;
using System.IO;

namespace DataAccess.Entities;

public class Folder
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public int? ParentFolderId { get; set; }

    public Folder? ParentFolder { get; set; }
    public ICollection<Folder>? SubFolders { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateChanged { get; set; }
    public ICollection<FileItem> Files { get; set; } = [];
    public string? Path { get; set; }
}

  