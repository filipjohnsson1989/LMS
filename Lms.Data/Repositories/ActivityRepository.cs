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
    public class ActivityRepository : IActivityRepository
    {

        private ApplicationDbContext db;
        public ActivityRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public bool ActivityExists(int id)
        {
            return db.Activities.Any(e => e.Id == id);
        }

        public async Task AddActivity(Activity activity)
        {
            await db.Activities.AddAsync(activity);
        }

        public async Task DeleteActivity(int id)
        {
            var activity = await db.Activities.FirstOrDefaultAsync(a => a.Id == id);
            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }
            db.Activities.Remove(activity);
        }

        public async Task<Activity> GetActivityId(int id)
        {
            var activity = await db.Activities.FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }
            return activity;
        }

        public async Task<IEnumerable<Activity>> GetAllActivities()
        {
            return await db.Activities.ToListAsync();
        }

        //Display all activities ordered by startdate
        public IEnumerable<Activity> SortActivitiesBySDate()
        {
            var activites = db.Activities.OrderByDescending(d => d.StartDate);
            return activites;
        }

        public void UpdateActivity(Activity activity)
        {
            db.Activities.Update(activity);
        }
    }
}
