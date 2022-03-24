using AutoMapper;
using Lms.Core.Entities;
using Lms.Core.ViewModels.Modules;

namespace Lms.Data.AutoMapper;

public class ModuleProfile : Profile
{
    public ModuleProfile()
    {

        CreateMap<Module, ModuleViewModel>();
        CreateMap<Module, CreateEditModuleViewModel>().ReverseMap();
        CreateMap<Module, SearchModuleViewModel>().ReverseMap();
    }
}
