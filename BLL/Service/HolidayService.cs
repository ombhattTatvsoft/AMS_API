using System.Net;
using System.Security.Claims;
using AutoMapper;
using BLL.IService;
using DAL.IRepository;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static BLL.Constants.Constant;

namespace BLL.Service;

public class HolidayService : IHolidayService
{
    private readonly IHolidayRepository _repository;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<UserService> _logger;

    private readonly IMapper _mapper;

    public int UpsertedBy { get; set; }

    public HolidayService(IHolidayRepository repository, IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<UserService> logger)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
        UpsertedBy = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public async Task<Response> GetHolidays()
    {
        try
        {
            List<Holiday> holidays = await _repository.GetAllAsync(x => !x.IsDeleted);
            return Response.Success(data: holidays.Select(h => new
            {
                id = h.HolidayId,
                title = h.HolidayName,
                start = h.HolidayDate.ToString("yyyy-MM-dd")
            }).ToList());
        }
        catch (Exception e)
        {
            _logger.LogError(e, GET_ALL_HOLIDAY_CATCH);
            return Response.Failed();
        }
    }

    public async Task<Response> SaveHolidayAsync(HolidayDTO model)
    {
        try
        {
            Holiday? holiday = (model.HolidayId == 0) ? new Holiday() : await _repository.GetAsync(x => x.HolidayId == model.HolidayId && !x.IsDeleted);
            if (model.HolidayId == 0)
            {
                holiday= _mapper.Map<Holiday>(model);
                holiday.CreatedBy = UpsertedBy;
                holiday.UpdatedBy = UpsertedBy;
                await _repository.AddAsync(holiday);
            }
            else
            {
                if (holiday == null)
                    return Response.Failed(HOLIDAY_NOT_FOUND,HttpStatusCode.NotFound);
                holiday = _mapper.Map(model, holiday);
                holiday.UpdatedAt = DateTime.Now;
                holiday.UpdatedBy = UpsertedBy;
                _repository.Update(holiday);
            }
            await _repository.SaveAsync();
            if (model.HolidayId == 0)
                return Response.Success(HOLIDAY_ADDED,HttpStatusCode.Created);
            return Response.Success(HOLIDAY_UPDATED, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, SAVE_HOLIDAY_CATCH);
            return Response.Failed();
        }
    }
    

    // public async Task<Response> DeleteHolidayAsync(int id)
    // {
    //     try
    //     {
    //         Holiday? holiday = await _repository.GetAsync(x => x.HolidayId == id && !x.IsDeleted);
    //         if (holiday == null)
    //         {
    //             return new Response(false, "Holiday not found.");
    //         }
    //         holiday.IsDeleted = true;
    //         _repository.Update(holiday);
    //         await _repository.SaveAsync();
    //         return new Response(true, "Holiday deleted successfully.");
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e, "Error deleting holiday with ID {Id}", id);
    //         return new Response(false, "An error occurred while deleting the holiday.");
    //     }
    // }

    // public async Task<Holiday?> GetHoliday(int id)
    // {
    //     try
    //     {
    //         return await _repository.GetAsync(x => x.HolidayId == id && !x.IsDeleted);
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e, "Error fetching holiday with ID {Id}", id);
    //         return null;
    //     }
    // }

}
