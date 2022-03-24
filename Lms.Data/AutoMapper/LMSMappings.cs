using AutoMapper;
using Lms.Core.Entities;
using Lms.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.AutoMapper
{
    public class LMSMappings : Profile
    {
        public LMSMappings()
        {
            CreateMap<Course, CourseOverViewModel>()
                .ForMember(
                dest => dest.ModuleCount,
                opt => opt.MapFrom(src => src.Modules.Count));

        }
    }
}
