using Lms.Core.Entities;
using Lms.Data.Data;

namespace Lms.Data.Repositories;

public class CourseRepositoryG : GenericRepository<Course>
{
    public CourseRepositoryG(ApplicationDbContext context) : base(context)
    {
    }
}
