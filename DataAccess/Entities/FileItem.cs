namespace DataAccess.Entities;

public class FileItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string FilePath { get; set; }
    public int? FolderId { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; } = null!;
    public int CreatedBy { get; set; } 

    public Folder Folder { get; set; } = null!;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateChanged { get; set; } = DateTime.UtcNow;
    public IEnumerable<FileVersion>? FileVersions { get; set; }
}