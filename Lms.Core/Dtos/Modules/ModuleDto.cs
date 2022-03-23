using Lms.Core.Dtos.Courses;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Dtos.Modules;

public class ModuleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string CourseName { get; set; } = default!;



}
