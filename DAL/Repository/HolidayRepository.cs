using DAL.IRepository;
using Entity.Models;

namespace DAL.Repository;

public class HolidayRepository : BaseRepository<Holiday>, IHolidayRepository
{
    private readonly AMSContext _context;

    public HolidayRepository(AMSContext context) : base(context)
    {
        _context = context;
    }
}
