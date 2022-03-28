using Lms.Core.ViewModels.ActivityTypes;
using Lms.Core.ViewModels.Documents;
using Lms.Core.ViewModels.Modules;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.ViewModels.Activities;

public class CreateEditActivityViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    public SearchModuleViewModel Module { get; set; } = default!;

    
    [Display(Name = "Activity Type")]
    public ActivityTypeViewModel ActivityType { get; set; } = default!;

    
    [Display(Name = "Upload Files")]
    public IEnumerable<IFormFile>? UploadFiles { get; set; } = default!;

    public IEnumerable<DocumentViewModel>? Documents { get; set; } = default!;
}
