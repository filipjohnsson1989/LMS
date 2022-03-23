using AutoMapper;
using Lms.Core.Dtos.Modules;
using Lms.Core.Entities;

namespace Lms.Data.AutoMapper;

public class ModuleProfile : Profile
{
    public ModuleProfile()
    {
        CreateMap<Module, ModuleDto>();
        CreateMap<Module, CreateEditModuleDto>().ReverseMap();
        CreateMap<Module, SearchModuleDto>().ReverseMap();

    }
}
