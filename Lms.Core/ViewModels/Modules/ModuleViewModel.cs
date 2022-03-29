using Lms.Core.ViewModels.Courses;
using Lms.Core.ViewModels.Documents;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.ViewModels.Modules;

public class ModuleViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string CourseName { get; set; } = default!;

    public IEnumerable<DocumentViewModel>? Documents { get; set; } = default!;

}
