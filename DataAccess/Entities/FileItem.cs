namespace DataAccess.Entities;

public class FileItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Path { get; set; }
    public int FolderId { get; set; }
    public Folder? Folder { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateChanged { get; set; }
}