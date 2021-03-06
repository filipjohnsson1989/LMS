using Lms.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Lms.Core.ViewModels
{
    public class UploadFileViewModel
    {
        //toDo Validation
        public IFormFile? Upload { get; set; }
        public string Url { get; set; }
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
        public int ActivityId { get; set; }
        public int ApplicationUserId { get; set; }

        public Course? Course { get; set; }
        public Module? Module { get; set; }
        public Activity? Activity { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
