using Lms.Core.Interfaces;
using Lms.Data.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories
{
    public class CourseSelector : ICourseSelector
    {
        private readonly ApplicationDbContext db;
        public CourseSelector(ApplicationDbContext db)
        {
            this.db = db;
        }

        public static int Course_Id { get; set; } = default!;

        public async Task<IEnumerable<SelectListItem>> GetSelectList()
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
