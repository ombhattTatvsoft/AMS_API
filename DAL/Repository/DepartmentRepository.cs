using DAL.IRepository;
using Entity.Models;

namespace DAL.Repository;

public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    private readonly AMSContext _context;

    public DepartmentRepository(AMSContext context) : base(context)
    {
        _context = context;
    }
}
