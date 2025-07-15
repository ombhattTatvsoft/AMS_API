using Entity.DTOs;
using Entity.Models;

namespace BLL.IService;

public interface IDepartmentService
{
    Task<Response> GetAllDepartmentsAsync();
    Task<Response> GetDepartmentAsync(int id);
    Task<Response> DeleteDepartmentAsync(int id);
    Task<Response> SaveDepartmentAsync(UpsertDepartmentDTO model);

}
