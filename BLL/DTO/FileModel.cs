namespace BLL.DTO;

public class FileModel
{
   

    public int Id { get; set; }
    public required string Name { get; set; }
    public required string? Path { get; set; }
    public int FolderId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateChanged { get; set; }

    public FolderModel Folder { get; set; } = null!;
    public string FileFormat { get; set; } = null!;
}