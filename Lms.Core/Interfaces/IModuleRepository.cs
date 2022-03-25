using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Interfaces
{
    public interface IModuleRepository
    {
        Task AddModule(Module module);
        void UpdateModule(Module module);
        Task DeleteModule(int id);
        bool ModuleExists(int id);
        Task<Module> GetModuleById(int id);
        Task<IEnumerable<Module>> GetAllModules();
        Task<IEnumerable<Module>> GetModulesByCourseIdAsync(int courseId);

    }
}
