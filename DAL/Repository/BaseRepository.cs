using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.IRepository;
using Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AMSContext _context;
    internal readonly DbSet<T> _dbSet;
    public BaseRepository(AMSContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if(includes!=null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.ToListAsync();
    }
    public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter=null,params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if(includes!=null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.FirstOrDefaultAsync();
    }
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }  
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

}
