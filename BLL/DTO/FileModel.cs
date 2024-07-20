namespace BLL.DTO;

public class FileModel
{
   

    public int Id { get; set; }
    public required string Name { get; set; }
    public required string? Path { get; set; }
    public int FolderId { get; set; }

    
}