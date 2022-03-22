using Lms.Core.Entities;

namespace Lms.Web.Models
{
    public class UpDownLoadCombinedTestViewModel
    {


        public UploadFileViewModel UploadFileViewModel { get; set; }
        public IEnumerable<Document> Documents { get; set; }
        public IEnumerable<ActivityType> ActivityTypes { get; set; }
    }
}
