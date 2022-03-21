using Lms.Core.Interfaces;
using Lms.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class UserRepository :IUserRepository
    {
        private ApplicationDbContext db;
        public UserRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public int GetCourse_By_UserId(string userid)
        {
            var c_id = db.Users
                            .Where(u => u.Id == userid)
                            .Select(s => new
                            {
                                courseid = s.Course.Id
                            }).FirstOrDefault();
            if(c_id== null)
            {
                throw new ArgumentNullException(nameof(c_id));
            }
            int course_id = c_id.courseid;

            return course_id;
        }
    }
}
