using Lms.Core.Entities;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories;

public class ActivityRepositoryG : GenericRepository<Activity>
{
    public ActivityRepositoryG(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Activity>> GetAllAsync(int? parentRelationId = null) => await base.GetAll()
                         .Include(activity => activity.Module)
                         .Include(activity => activity.ActivityType)
                         .ToListAsync();

    public override async Task<Activity?> GetAsync(int id)
    {
        return await context.Activities
            .Include(activity => activity.Module)
            .Include(activity => activity.ActivityType)
            .Include(activity => activity.Documents)
            .FirstOrDefaultAsync(activity => activity.Id == id);

    }
}
