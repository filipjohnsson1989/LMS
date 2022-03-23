using Lms.Core.Dtos.Modules;
using Lms.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Dtos.Activities;

public class CreateEditActivityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [Display(Name = "Module")]
    public SearchModuleDto Module { get; set; } = default!;
    [Display(Name = "Activity Type")]

    public ActivityType ActivityType { get; set; } = default!;




}
