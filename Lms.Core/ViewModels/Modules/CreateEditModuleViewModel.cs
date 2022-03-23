using Lms.Core.ViewModels.Courses;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.ViewModels.Modules;

public class CreateEditModuleViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [Display(Name = "Course")]
    public SearchCourseViewModel Course { get; set; } = default!;



}
