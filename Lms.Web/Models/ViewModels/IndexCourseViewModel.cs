namespace Lms.Web.Models.ViewModels
{
    public class IndexCourseViewModel
    {
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public string Name { get; set; } = default!;
    }
}
