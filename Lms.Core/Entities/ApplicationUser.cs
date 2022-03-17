
using Microsoft.AspNetCore.Identity;

namespace Lms.Core.Entities;

public class ApplicationUser : IdentityUser
{
    //public string Name { get; set; } = default!;
    public Course? Course { get; set; }
    public int? CourseId { get; set; }

}

