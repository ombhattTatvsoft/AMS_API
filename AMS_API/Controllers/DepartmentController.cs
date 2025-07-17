using BLL.Helpers;
using BLL.IService;
using Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;
    private readonly ActionMapper actionMapper;
    public DepartmentController(IDepartmentService service)
    {
        _service = service;
        actionMapper = new ActionMapper();
    }

    [HttpGet("get-departments")]
    public async Task<IActionResult> GetDepartments()
    {
        Response response = await _service.GetAllDepartmentsAsync();
        return actionMapper.MapToActionResult(response);
    }

    [HttpGet("get-department/{id}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        Response response = await _service.GetDepartmentAsync(id);
        return actionMapper.MapToActionResult(response);
    }

    [HttpPost("save-department")]
    public async Task<IActionResult> SaveDepartment(UpsertDepartmentDTO model)
    {
        Response response = await _service.SaveDepartmentAsync(model);
        return actionMapper.MapToActionResult(response);
    }

    [HttpDelete("delete-department/{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        Response response = await _service.DeleteDepartmentAsync(id);
        return actionMapper.MapToActionResult(response);
    }
}
