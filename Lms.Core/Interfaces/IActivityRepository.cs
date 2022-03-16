using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Interfaces
{
    public interface IActivityRepository
    {
        Task AddActivity(Activity activity);
        void UpdateActivity(Activity activity);
        Task DeleteActivity(int id);
        bool ActivityExists(int id);
        Task<Activity> GetActivityId(int id);
        Task<IEnumerable<Activity>> GetAllActivities();

        //Display all by latest activities first
        IEnumerable<Activity> SortActivitiesBySDate();
    }
}
