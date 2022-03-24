using Lms.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Lms.Core.ViewModels
{
    public class UploadFileViewModel
    {
        //toDo Validation
        public IFormFile? Upload { get; set; }

        public Course? Course { get; set; }
        public Module? Module { get; set; }
        public Activity? Activity { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
