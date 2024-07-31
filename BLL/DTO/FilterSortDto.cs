using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class FilterSortDto
    {
        public string SortBy { get; set; }
        public bool Ascending { get; set; } = true;
        public string FileType { get; set; }
        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }
    }

}
