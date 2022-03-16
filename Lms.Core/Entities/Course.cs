namespace Lms.Core.Entities;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public ICollection<Module> Modules { get; set; } = default!;
    public Document Document { get; set; } = default!;
}
