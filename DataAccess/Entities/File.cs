// FileManager.DataAccess/Entities/File.cs

namespace DataAccess.Entities;

public class File
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public int FolderId { get; set; }
    public Folder Folder { get; set; }
}