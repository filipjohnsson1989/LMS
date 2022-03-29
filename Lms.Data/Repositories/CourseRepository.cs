using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext db;
    public CourseRepository(ApplicationDbContext db)
    {
        this.db = db ?? throw new NullReferenceException(nameof(db));
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
        var course = await db.Courses
                    .Include(m=>m.Modules)
                    .Include(d=>d.Documents)
                    .ThenInclude(a=>a.Activity)
                    .ThenInclude(a=>a.ActivityType)
                   .FirstOrDefaultAsync(c => c.Id == id);

        foreach (var module in course.Modules)
        {
            db.Remove(module);
        }
        foreach (var document in course.Documents)
        {
            db.Remove(document.Activity);
            db.Remove(document.Activity.ActivityType);
            db.Remove(document);
        }
        
        if (course == null)
        {
            throw new NullReferenceException(nameof(course));
        }
        db.Courses.Remove(course);

    }
    public async Task<IEnumerable<Course>> GetAll()
    {
        return await db.Courses
            .Include(m=>m.Modules)
                .ThenInclude(a=>a.Activities)
                  .ThenInclude(a=>a.ActivityType)
            .Include(m => m.Modules)
                 .ThenInclude(a => a.Activities)
                      .ThenInclude(a => a.Documents)
            .Include(m =>m.Modules)
                .ThenInclude(a=>a.Documents)
            .Include(m => m.Documents)
                .ThenInclude(u=>u.User)
            .Include(u=>u.Users)
            //.Include(m => m.Users)
            //    .ThenInclude(u => u.Documents)
            .ToListAsync();
    }
    public async Task <Course> GetAllbyId(int id)
    {
        var course = await db.Courses
            .Include(m => m.Modules)
                .ThenInclude(a => a.Activities)
                  .ThenInclude(a => a.ActivityType)
            .Include(m => m.Modules)
                 .ThenInclude(a => a.Activities)
                      .ThenInclude(a => a.Documents)
            .Include(m => m.Modules)
                .ThenInclude(a => a.Documents)
            .Include(m => m.Documents)
                .ThenInclude(u=>u.User)
            .Include(u=>u.Users)
            .FirstOrDefaultAsync(c => c.Id == id);
            

        //if(course==null)
        //    throw new ArgumentException(nameof(course));
            //.Include(m => m.Users)
            //    .ThenInclude(u => u.Documents)
            return course;
    }

  
   
    public async Task<IEnumerable<Course>> GetAllCourses()
    {
        return await db.Courses.ToListAsync();
    }
    public async Task<Course> GetCourseByIdAsync(int id)
    {
        var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == id);
        if (course == null)
        {
            throw new NullReferenceException(nameof(course));
        }
        return (course);
    }
        public async Task<Course> GetCourseById_IncludeModulesAsync(int id)
    {
        var course = await db.Courses.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == id);
        if (course == null)
        {
            throw new NullReferenceException(nameof(course));
        }
        return (course);
    }
    public async Task<Course> GetCourseById_IncludeUsersAsync(int id)
    {
        var course = await db.Courses.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == id);
        if (course == null)
        {
            throw new NullReferenceException(nameof(course));
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
            throw new NullReferenceException(nameof(course));
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
