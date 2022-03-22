using Lms.Core.Entities;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories;

public class ModuleRepositoryG : GenericRepository<Module>
{
    public ModuleRepositoryG(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Module>> GetAllAsync() => await base.GetAll()
                         .Include(module => module.Course)
                         .ToListAsync();

    public override async Task<Module?> GetAsync(int id)
    {
        return await context.Modules
            .Include(module => module.Course)
            .FirstOrDefaultAsync(module => module.Id == id);

    }
}
