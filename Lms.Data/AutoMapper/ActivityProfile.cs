using AutoMapper;
using Lms.Core.Dtos.Activities;
using Lms.Core.Entities;

namespace Lms.Data.AutoMapper;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<Activity, ActivityDto>();
        CreateMap<Activity, CreateEditActivityDto>().ReverseMap();
    }
}
