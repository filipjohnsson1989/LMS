using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Services
{
    public  class CourseSelector : ICourseSelector
    {
        private readonly ApplicationDbContext db;
        public CourseSelector(ApplicationDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<SelectListItem>> GetSelectListAsync()
        {
            return await db.Courses.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            })
                .ToListAsync();
        }
    }
}
