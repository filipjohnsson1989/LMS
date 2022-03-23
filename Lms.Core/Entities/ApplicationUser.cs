
using Microsoft.AspNetCore.Identity;

namespace Lms.Core.Entities;

public class ApplicationUser : IdentityUser
{
    //public string Name { get; set; } = default!;
    public int? CourseId { get; set; }
    public Course? Course { get; set; }


}

