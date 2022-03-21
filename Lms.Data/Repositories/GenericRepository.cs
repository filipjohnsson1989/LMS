using Lms.Core.Interfaces;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Lms.Data.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private protected ApplicationDbContext context;

    public GenericRepository(ApplicationDbContext context)
    {
        context = context;
    }
    public async Task<T> AddAsync(T entity)
    {
        var result = await context
                .AddAsync(entity);
        return result.Entity;
    }



    public void Delete(T entity)
    {
        context
            .Remove(entity);


    }

    public async Task<bool> ExistAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return context.Set<T>()
                .AsQueryable()
                .Where(predicate).ToList();
    }

    public async Task<T>? GetAsync(int id)
    {
        return context.Find<T>(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return context.Set<T>()
                .AsQueryable()
                .ToList();
    }

    public async Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}
