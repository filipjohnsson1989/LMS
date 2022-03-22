using Lms.Core.Interfaces;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;
namespace Lms.Data.Repositories
{
    public class CourseSelector : ICourseSelector
    {
        private readonly ApplicationDbContext db;
        public CourseSelector(ApplicationDbContext db)
        {
            this.db = db;
        }

        public int Course_Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<IEnumerable<SelectListItem>> GetSelectList()
        {
            return await db.Courses.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            })
                .ToListAsync();
        }
    }
}
