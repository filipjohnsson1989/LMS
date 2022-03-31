namespace Lms.Core.ViewModels.Modules;

public class IndexModuleViewModel
{
    public int? CourseId { get; set; }
    public string? CourseName { get; set; }
    public IEnumerable<Lms.Core.ViewModels.Modules.ModuleViewModel> Modules { get; set; } = default!;
}
