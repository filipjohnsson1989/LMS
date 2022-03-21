using Lms.Core.Entities;

namespace Lms.Web.Models
{
    public class UpDownLoadCombinedTestViewModel
    {
      

            public UploadFileViewModel UploadFileViewModel { get; set; }
            public ICollection<Document> Documents { get; set; }
        public List<Lms.Core.Entities.ActivityType> activityTypes { get; set; }
    }
}
