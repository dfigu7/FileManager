using DataAccess.Entities;

namespace DataAccess.DTO;

public class FileModel
{
   
    
    public required string Name { get; set; }
    
    public int FolderId { get; set; }
  
    public ICollection<FileVersion> FileVersions { get; set; } = new List<FileVersion>();

}