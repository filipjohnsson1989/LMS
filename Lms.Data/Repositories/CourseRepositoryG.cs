using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories;

public class CourseRepositoryG : GenericRepository<Course>
{
    public CourseRepositoryG(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Course?> GetInculdeDocumentsAsync(int id)
    {
        return await context.Courses
            .Include(course => course.Documents)
            .FirstOrDefaultAsync(course => course.Id == id);

    }

    public async Task<IEnumerable<Course>> GetAllInculdeDocumentsAndModulesAsync()
    {
        return await context.Courses
            .Include(course => course.Documents)
            .Include(course => course.Modules)
            .ToListAsync();

    }

}
