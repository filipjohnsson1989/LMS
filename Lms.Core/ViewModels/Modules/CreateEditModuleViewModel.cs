using Lms.Core.ViewModels.Courses;
using Lms.Core.ViewModels.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.ViewModels.Modules;

public class CreateEditModuleViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    [Remote(action: "VerifyStartdate", controller: "Modules",
            AdditionalFields = nameof(CourseId))]
    public DateTime StartDate { get; set; }
    [Remote(action: "VerifyEnddate", controller: "Modules",
           AdditionalFields = nameof(StartDate))]
    public DateTime EndDate { get; set; }

    [Display(Name = "Course")]
    public SearchCourseViewModel Course { get; set; } = default!;

    [Display(Name = "Upload Files")]
    public IEnumerable<IFormFile>? UploadFiles { get; set; } = default!;

    public IEnumerable<DocumentViewModel>? Documents { get; set; } = default!;

    public int CourseId { get; set; }

}
