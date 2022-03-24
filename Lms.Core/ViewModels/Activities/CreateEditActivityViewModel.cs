using Lms.Core.ViewModels.ActivityTypes;
using Lms.Core.ViewModels.Modules;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.ViewModels.Activities;

public class CreateEditActivityViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [Display(Name = "Module")]
    public SearchModuleViewModel Module { get; set; } = default!;

    [Display(Name = "Activity Type")]
    public ActivityTypeViewModel ActivityType { get; set; } = default!;




}
