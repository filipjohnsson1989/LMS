using AutoMapper;
using Lms.Core.ViewModels.Modules;
using Lms.Core.Entities;

namespace Lms.Data.AutoMapper;

public class ModuleProfile : Profile
{
    public ModuleProfile()
    {
        CreateMap<Module, ModuleViewModel>();
        CreateMap<Module, CreateEditModuleViewModel>().ReverseMap();
    }
}
