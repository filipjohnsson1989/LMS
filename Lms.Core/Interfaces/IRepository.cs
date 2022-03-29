using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Lms.Core.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    Task<T> AddAsync(T entity);
    void Delete(T entity);
    Task<bool> ExistAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetAsync(int id);

    IQueryable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();
    Task SaveChangesAsync();
    T Update(T entity);

}
