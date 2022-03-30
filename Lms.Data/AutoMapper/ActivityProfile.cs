using AutoMapper;
using Lms.Core.Entities;
using Lms.Core.ViewModels.Activities;

namespace Lms.Data.AutoMapper;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<Activity, ActivityViewModel>();
        CreateMap<Activity, CreateEditActivityViewModel>().ReverseMap();
    }
}
