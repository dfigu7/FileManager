using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities;

public class Folder
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public int? ParentFolderId { get; set; }
    public required ICollection<FileItem> Files { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateChanged { get; set; }
}

  