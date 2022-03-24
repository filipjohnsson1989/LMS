using AutoMapper;
using Lms.Core.ViewModels.ActivityTypes;

namespace Lms.Data.AutoMapper;

public class ActivittTypeProfile : Profile
{
    public ActivittTypeProfile()
    {
        CreateMap<ActivityType, ActivityTypeViewModel>().ReverseMap();
    }
}
