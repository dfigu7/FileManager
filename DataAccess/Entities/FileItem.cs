namespace DataAccess.Entities;

public class FileItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string FilePath { get; set; }
    public int? FolderId { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; } = null!;

    public Folder Folder { get; set; } = null!;
    public DateTime DateCreated { get; set; }
    public DateTime DateChanged { get; set; }
}