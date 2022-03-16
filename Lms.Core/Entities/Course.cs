namespace Lms.Core.Entities;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public ICollection<Module> Modules { get; set; } = default!;
    public ICollection<Activity> Activities { get; set; } =default!;
    public ICollection<Document> Documents { get; set; } = default!;
    public ICollection<ApplicationUser> Users { get; set; } = default!;

}
