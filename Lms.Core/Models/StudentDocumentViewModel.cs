using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models
{
    public class StudentDocumentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime UploadDate { get; set; }
        public string UserId { get; set; }
    }
}
