using Lms.Core.Entities;

namespace Lms.Core.ViewModels
{
    public class UpDownLoadCombinedTestViewModel
    {


        public UploadFileViewModel UploadFileViewModel { get; set; }
        public IEnumerable<Document> Documents { get; set; }
        public IEnumerable<ActivityType> ActivityTypes { get; set; }
    }
}
