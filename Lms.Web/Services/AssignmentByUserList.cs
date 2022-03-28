using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lms.Web.Services
{
    public class AssignmentByUserList : IAssignmentByUserList
    {
        private readonly ApplicationDbContext db;
        public AssignmentByUserList(ApplicationDbContext db)
        {
            this.db = db;
        }


        public async Task<IEnumerable<ApplicationUser>> GetStudentListAsync(int activityId)
        {
            var activity = await db.Activities.Include(a => a.Module.Course.Users).FirstAsync(a => a.Id == activityId);
            
            var students = activity.Module.Course.Users.ToList();
            
            //if(course==null)
            //    throw new ArgumentException(nameof(course));
            //.Include(m => m.Users)
            //    .ThenInclude(u => u.Documents)
            return students;
        }
        
    }
}
