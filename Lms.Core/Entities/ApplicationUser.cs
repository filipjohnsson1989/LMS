using Microsoft.AspNet.Identity.EntityFramework;

namespace Lms.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        //public int? CourseId { get; set; } = null;

    }
}
