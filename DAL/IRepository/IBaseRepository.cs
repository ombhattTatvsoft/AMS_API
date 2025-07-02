using System.Linq.Expressions;

namespace DAL.IRepository;

public interface IBaseRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter=null,params Expression<Func<T, object>>[]? includes);
    Task<T?> GetAsync(Expression<Func<T, bool>>? filter=null,params Expression<Func<T, object>>[]? includes);
    Task AddAsync(T entity);
    void Update(T entity);
    Task SaveAsync();
}
