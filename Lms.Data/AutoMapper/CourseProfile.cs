using AutoMapper;
using Lms.Core.ViewModels.Courses;
using Lms.Core.Entities;

namespace Lms.Data.AutoMapper;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseViewModel>().ReverseMap();
        CreateMap<Course, SearchCourseViewModel>().ReverseMap();
    }
}
