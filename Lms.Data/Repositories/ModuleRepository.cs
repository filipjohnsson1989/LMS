using Lms.Core.Entities;
using Lms.Core.Interfaces;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class ModuleRepository : IModuleRepository
    {

        private ApplicationDbContext db;
        public ModuleRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task AddModule(Module module)
        {
            await db.Modules.AddAsync(module);
        }

        public async Task DeleteModule(int id)
        {
            var module = await db.Modules.FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }
            db.Modules.Remove(module);
        }

        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await db.Modules.ToListAsync();
        }
       
        public async Task<Module> GetModuleById(int id)
        {
            var module = await db.Modules.FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }
            return module;
        }

        public bool ModuleExists(int id)
        {
            return db.Modules.Any(e => e.Id == id);
        }

        public void UpdateModule(Module module)
        {
            db.Modules.Update(module);
        }
    }
}
