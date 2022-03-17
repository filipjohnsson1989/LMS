using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Interfaces
{
    public interface IActivityTypeRepository
    {
        Task AddActivityType(ActivityType activitytype);
        void UpdateActivityType(ActivityType activitytype);
        Task DeleteActivityType(int id);
        bool ActivityTypeExists(int id);
        Task<ActivityType> GetActivityTypeById(int id);
        Task<IEnumerable<ActivityType>> GetAllActivityTypes();
    }
}
