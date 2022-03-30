using AutoMapper;
using Lms.Core.ViewModels.ActivityTypes;

namespace Lms.Data.AutoMapper;

public class ActivityTypeProfile : Profile
{
    public ActivityTypeProfile()
    {
        CreateMap<ActivityType, ActivityTypeViewModel>().ReverseMap();
    }
}
