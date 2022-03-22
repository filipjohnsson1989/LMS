using System.Web.Mvc;

namespace Lms.Core.Interfaces
{
    public interface ICourseSelector
    {
        public int Course_Id { get; set; }
        public Task<IEnumerable<SelectListItem>> GetSelectList();
    }
}