using Lms.Core.Entities;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories;

public class ModuleRepositoryG : GenericRepository<Module>
{
    public ModuleRepositoryG(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Module>> GetAllAsync(int? parentRelationId = null)
    {
        var query =
          base.GetAll();

        if(parentRelationId is not null) {
            query = query.Where(module => module.CourseId == parentRelationId);
        }

        return await query.Include(module => module.Course)
                          .ToListAsync();
    }

    public override async Task<Module?> GetAsync(int id)
    {
        return await context.Modules
            .Include(module => module.Course)
            .Include(activity => activity.Documents)
            .FirstOrDefaultAsync(module => module.Id == id);

    }
}
