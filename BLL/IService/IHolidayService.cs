using Entity.DTOs;

namespace BLL.IService;

public interface IHolidayService
{
    Task<Response> GetHolidays();
    // Task<Holiday?> GetHoliday(int id);
    Task<Response> SaveHolidayAsync(HolidayDTO model);
    // Task<Response> DeleteHolidayAsync(int id);
}
