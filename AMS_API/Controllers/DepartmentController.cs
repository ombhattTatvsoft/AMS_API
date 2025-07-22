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
    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet("get-departments")]
    public async Task<IActionResult> GetDepartments()
    {
        Response response = await _service.GetAllDepartmentsAsync();
        return this.MapToActionResult(response);
    }

    [HttpGet("get-department/{id}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        Response response = await _service.GetDepartmentAsync(id);
        return this.MapToActionResult(response);
    }

    [HttpPost("save-department")]
    public async Task<IActionResult> SaveDepartment(UpsertDepartmentDTO model)
    {
        Response response = await _service.SaveDepartmentAsync(model);
        return this.MapToActionResult(response);
    }

    [HttpDelete("delete-department/{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        Response response = await _service.DeleteDepartmentAsync(id);
        return this.MapToActionResult(response);
    }
}
