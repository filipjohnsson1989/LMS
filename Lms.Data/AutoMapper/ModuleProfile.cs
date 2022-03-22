using AutoMapper;
using Lms.Core.Dtos.Module;
using Lms.Core.Entities;

namespace Lms.Data.AutoMapper;

public class ModuleProfile : Profile
{
    public ModuleProfile()
    {
        CreateMap<Module, ModuleDto>();
        CreateMap<Module, CreateEditModuleDto>().ReverseMap();
    }
}
