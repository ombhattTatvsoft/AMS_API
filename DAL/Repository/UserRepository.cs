using DAL.IRepository;
using Entity.Models;

namespace DAL.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AMSContext _context;

    public UserRepository(AMSContext context) : base(context)
    {
        _context = context;
    }

}
