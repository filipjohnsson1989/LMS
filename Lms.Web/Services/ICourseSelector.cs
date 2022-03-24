using Microsoft.AspNetCore.Mvc.Rendering;
#nullable disable

namespace Lms.Web.Services
{
    public interface ICourseSelector
    {
        
        public Task<IEnumerable<SelectListItem>> GetSelectList();
    }
}

