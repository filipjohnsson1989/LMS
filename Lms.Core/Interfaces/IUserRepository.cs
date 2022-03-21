using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Interfaces
{
    public interface IUserRepository
    {
         int GetCourse_By_UserId(string userid);
    }
}
