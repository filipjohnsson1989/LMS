using Lms.Core.Dtos.Modules;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Dtos.Activities;

public class ActivityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string ModuleName { get; set; } = default!;
    public string ActivityTypeName { get; set; } = default!;




}
