using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models
{
     public class CourseOverViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public int ModuleCount { get; set; }
        public ICollection<Module> Modules { get; set; } = default!;
        public List<string> UserName { get; set; }
        public List<string> Email { get; set; }
    }
}
