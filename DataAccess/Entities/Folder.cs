// FileManager.DataAccess/Entities/Folder.cs

namespace DataAccess.Entities;

public class Folder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentFolderId { get; set; }
    public Folder ParentFolder { get; set; }
    public ICollection<File> Files { get; set; }
    public ICollection<Folder> SubFolders { get; set; }
}