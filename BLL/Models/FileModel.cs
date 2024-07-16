// FileManager.BLL/Models/FileModel.cs

namespace BLL.Models;

public class FileModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public int FolderId { get; set; }
}