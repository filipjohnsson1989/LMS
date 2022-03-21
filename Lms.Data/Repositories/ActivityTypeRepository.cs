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
    public class ActivityTypeRepository : GenericRepository<ActivityType>
    {
        public ActivityTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
