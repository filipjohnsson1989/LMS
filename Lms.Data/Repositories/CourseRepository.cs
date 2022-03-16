using Lms.Core.Entities;
using Lms.Core.Interfaces;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private ApplicationDbContext db;
        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task AddCourse(Course course)
        {
            await db.Courses.AddAsync(course);
        }

        public bool CourseExists(int id)
        {
            return db.Courses.Any(e => e.Id == id);
        }

        public async Task DeleteCourse(int id)
        {
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            db.Courses.Remove(course);

        }
        public async Task<IEnumerable<Course>> GetAll()
        {
            return await db.Courses.Include(m=>m.Modules)
                .ThenInclude(a=>a.Activities)
                .ThenInclude(d=>d.Documents)
                .ToListAsync();
        }
        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await db.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseById(int id)
        {
            var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            return (course);
        }

        //Sort the Modules of a Course through Name; Ascending or Descending.
        public async Task<Course> GetCourseById_Include_SortModule(int courseId, string sortOrder)
        {
            var course = await db.Courses.Include(c => c.Modules)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            if (sortOrder == "asc")
            {
                course.Modules = course.Modules.OrderBy(m => m.Name).ToList();
                return course;
            }
            else
            {
                course.Modules = course.Modules.OrderByDescending(m => m.Name).ToList();
                return course;
            }
        }

        public void UpdateCourse(Course course)
        {
            db.Courses.Update(course);

        }
    }
}
