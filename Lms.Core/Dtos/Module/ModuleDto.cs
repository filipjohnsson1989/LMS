using Lms.Core.Dtos.Course;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Dtos.Module;

public class ModuleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string CourseName { get; set; } = default!;



}
