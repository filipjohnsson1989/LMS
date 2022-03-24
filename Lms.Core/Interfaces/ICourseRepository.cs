using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Interfaces
{
     public interface ICourseRepository
    {
        
        Task AddCourse(Course course);
        void UpdateCourse(Course course);
        Task DeleteCourse(int id);
        bool CourseExists(int id);
        Task<Course> GetCourseById(int id);
        Task<Course> GetAllbyId(int id);
        Task<IEnumerable<Course>> GetAll();
        Task<IEnumerable<Course>> GetAllCourses();

        //Sort the Modules of a Course through Name; Ascending or Descending.
        Task<Course> GetCourseById_Include_SortModule(int courseId, string sortOrder);
        Task<Course> GetCourseByIdWithUsers(int id);
    }
}
