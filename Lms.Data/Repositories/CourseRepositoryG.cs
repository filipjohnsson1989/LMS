using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories;

public class CourseRepositoryG : GenericRepository<Course>
{
    public CourseRepositoryG(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Course?> GetAsync(int id)
    {
        return await context.Courses
            .Include(course => course.Documents)
            .FirstOrDefaultAsync(course => course.Id == id);

    }
}
