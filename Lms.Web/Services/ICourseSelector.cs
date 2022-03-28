using Microsoft.AspNetCore.Mvc.Rendering;
#nullable disable

namespace Lms.Web.Services
{
    public interface ICourseSelector
    {
        
        Task<IEnumerable<SelectListItem>> GetSelectListAsync();
    }
}

