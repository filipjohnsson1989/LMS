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
    public class ActivityTypeRepository : IActivityTypeRepository
    {

        private ApplicationDbContext db;
        public ActivityTypeRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task AddActivityType(ActivityType activitytype)
        {
            await db.ActivityTypes.AddAsync(activitytype);
        }
        public void UpdateActivityType(ActivityType activitytype)
        {
            db.ActivityTypes.Update(activitytype);
        }
        public async Task DeleteActivityType(int id)
        {
            var activitytype = await db.ActivityTypes.FirstOrDefaultAsync(a => a.Id == id);
            if (activitytype == null)
            {
                throw new ArgumentNullException(nameof(activitytype));
            }
            db.ActivityTypes.Remove(activitytype);
        }

        public bool ActivityTypeExists(int id)
        {
            return db.ActivityTypes.Any(e => e.Id == id);
        }
        public async Task<ActivityType> GetActivityTypeById(int id)
        {
            var activitytype = await db.ActivityTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (activitytype == null)
            {
                throw new ArgumentNullException(nameof(activitytype));
            }
            return activitytype;
        }

        public async Task<IEnumerable<ActivityType>> GetAllActivityTypes()
        {
            return await db.ActivityTypes.ToListAsync();
        }

    }
}
