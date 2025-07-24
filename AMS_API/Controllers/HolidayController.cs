using BLL.Helpers;
using BLL.IService;
using Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _service;

        public HolidayController(IHolidayService service)
        {
            _service = service;
        }

        [HttpGet("get-holidays")]
        public async Task<IActionResult> GetHolidays()
        {
            Response response = await _service.GetHolidays();
            return this.MapToActionResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("save-holiday")]
        public async Task<IActionResult> SaveHoliday(HolidayDTO model)
        {
            Response response = await _service.SaveHolidayAsync(model);
            return this.MapToActionResult(response);
        }
    }
}