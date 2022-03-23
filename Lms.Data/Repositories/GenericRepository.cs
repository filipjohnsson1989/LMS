using Lms.Core.Interfaces;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Lms.Data.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private protected ApplicationDbContext context;
    private DbSet<T> DbSet => context.Set<T>();

    public GenericRepository(ApplicationDbContext context) => this.context = context ?? throw new ArgumentNullException(nameof(context));
    public virtual async Task<T> AddAsync(T entity) => (await context.AddAsync(entity)).Entity;

    public virtual void Delete(T entity) => context.Remove(entity);

    public virtual async Task<bool> ExistAsync(int id) => await DbSet.AnyAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await DbSet.AsQueryable()
                      .Where(predicate)
                      .ToListAsync();


    public virtual async Task<T?> GetAsync(int id) => await context.FindAsync<T>(id);

    public virtual IQueryable<T> GetAll()
        => DbSet.AsQueryable();

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await this.GetAll()
                     .ToListAsync();

    public virtual async Task SaveChangesAsync() => await context.SaveChangesAsync();

    public T Update(T entity)
        => context.Update(entity)
                  .Entity;
}
