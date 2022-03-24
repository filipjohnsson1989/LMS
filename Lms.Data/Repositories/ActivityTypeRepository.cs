namespace Lms.Data.Repositories;

public class ActivityTypeRepository : GenericRepository<ActivityType>
{
    public ActivityTypeRepository(ApplicationDbContext context) : base(context)
    {
    }

}
