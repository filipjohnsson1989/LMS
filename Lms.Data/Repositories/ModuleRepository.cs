using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories;

public class ModuleRepository : IModuleRepository
{

    private readonly ApplicationDbContext db;
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
            throw new NullReferenceException(nameof(module));
        }
        db.Modules.Remove(module);
    }

    public async Task<IEnumerable<Module>> GetAllModules()
    {
        return await db.Modules.ToListAsync();
    }

    public async Task<IEnumerable<Module>> GetAllModulesByCourseId(int courseId)
    {
       var module =  db.Modules.Where(c=>c.CourseId==courseId).AsQueryable();
        return module;
    }


    public async Task<Module> GetModuleById(int id)
    {
        var module = await db.Modules.FirstOrDefaultAsync(m => m.Id == id);
        if (module == null)
        {
            throw new NullReferenceException(nameof(module));
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
