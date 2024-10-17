using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class FileVersion
    {
        public int Id { get; set; }
        public int FileItemId { get; set; }
        public required string Name { get; set; }
        public required string FilePath { get; set; }
        public int? FolderId { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; } = null!;
        public int CreatedBy { get; set; }

        public Folder Folder { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime DateChanged { get; set; }
        public DateTime VersionDate { get; set; }
        public FileItem FileItem { get; set; }
    }

}
