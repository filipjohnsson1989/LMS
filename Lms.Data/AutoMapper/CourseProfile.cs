using AutoMapper;
using Lms.Core.Dtos.Courses;
using Lms.Core.Entities;

namespace Lms.Data.AutoMapper;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Course, SearchCourseDto>().ReverseMap();
    }
}
