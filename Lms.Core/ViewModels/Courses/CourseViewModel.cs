using Lms.Core.ViewModels.Documents;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.ViewModels.Courses;

public class CourseViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }

    [Display(Name = "Upload Files")]
    public IEnumerable<IFormFile>? UploadFiles { get; set; } = default!;

    public IEnumerable<DocumentViewModel>? Documents { get; set; } = default!;

}
