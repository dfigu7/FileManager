// FileManager.DataAccess/Entities/Folder.cs

using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities;

public class Folder
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentFolderId { get; set; }
    public Folder ParentFolder { get; set; }
    public ICollection<FileItem> Files { get; set; }
    public ICollection<Folder> SubFolders { get; set; }
}