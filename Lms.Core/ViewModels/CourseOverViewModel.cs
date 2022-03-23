#nullable disable
namespace Lms.Core.ViewModels;

public class CourseOverViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public int ModuleCount { get; set; }
    public ICollection<Lms.Core.Entities.Module> Modules { get; set; } = default!;
    public List<string> UserName { get; set; }
    public List<string> Email { get; set; }
}
