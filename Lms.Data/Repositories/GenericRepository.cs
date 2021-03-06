using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Lms.Data.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class, IEntity
{
    private protected ApplicationDbContext context;
    private DbSet<T> DbSet => context.Set<T>();

    public GenericRepository(ApplicationDbContext context) => this.context = context ?? throw new ArgumentNullException(nameof(context));
    public virtual async Task<T> AddAsync(T entity) => (await context.AddAsync(entity)).Entity;

    public virtual void Delete(T entity) => context.Remove(entity);

    public virtual async Task<bool> ExistAsync(int id) => await DbSet.AnyAsync();

    public virtual async Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate)
        => await DbSet.AsQueryable()
                      .Where(predicate)
                      .ToListAsync();


    public virtual async Task<T?> GetAsync(int id) => await context.FindAsync<T>(id);

    public virtual IQueryable<T> GetAll()
        => DbSet
        .OrderByDescending(record => record.Id) // TakeLast(10)
        .Take(10) // TakeLast(10)
        .AsQueryable();

    public virtual async Task<IEnumerable<T>> GetAllAsync(int? parentRelationId = null)
        => await this.GetAll()
                     .ToListAsync();

    public virtual async Task SaveChangesAsync() => await context.SaveChangesAsync();

    public T Update(T entity)
        => context.Update(entity)
                  .Entity;
}
