using AutoMapper;
using Lms.Core.ViewModels.Courses;
using Lms.Core.Entities;

namespace Lms.Data.AutoMapper;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseViewModel>().ReverseMap();
        //CreateMap<CourseViewModel, Course>()
        //    .ForMember(dest => dest.Modules.Count(), opt => opt.MapFrom(src => src.CountOfModules)); 
        CreateMap<Course, SearchCourseViewModel>().ReverseMap();
    }
}
