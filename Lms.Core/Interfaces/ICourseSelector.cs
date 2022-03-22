
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lms.Core.Interfaces
{
    public interface ICourseSelector
    {
        public static int Course_Id { get; set; }
        public Task<IEnumerable<SelectListItem>> GetSelectList();
        
    }
}