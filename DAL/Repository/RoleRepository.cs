using DAL.IRepository;
using Entity.Models;

namespace DAL.Repository;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    private readonly AMSContext _context;

    public RoleRepository(AMSContext context) : base(context)
    {
        _context = context;
    }

}

