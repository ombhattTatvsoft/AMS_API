using System.Net;
using System.Security.Claims;
using AutoMapper;
using static BLL.Constants.Constant;
using BLL.IService;
using DAL.IRepository;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BLL.Service;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<DepartmentService> _logger;

    private readonly IMapper _mapper;

    public int UpsertedBy { get; set; }

    public DepartmentService(IDepartmentRepository repository, IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<DepartmentService> logger)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
        UpsertedBy = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public async Task<Response> GetAllDepartmentsAsync()
    {
        try
        {
            List<Department> departments = await _repository.GetAllAsync(d => !d.IsDeleted, x => x.Users);
            return Response.Success(data: _mapper.Map<List<DepartmentDTO>>(departments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GET_ALL_DEPT_CATCH);
            return Response.Failed();
        }
    }

    public async Task<Response> GetDepartmentAsync(int id)
    {
        try
        {
            Department? department = await _repository.GetAsync(x => x.DepartmentId == id && !x.IsDeleted);
            return department == null ? Response.Failed(statusCode:HttpStatusCode.NotFound) : Response.Success(data:department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GET_DEPT_CATCH, id);
            return Response.Failed();
        }
    }

    public async Task<Response> SaveDepartmentAsync(UpsertDepartmentDTO model)
    {
        try
        {
            Department? departmentWithSameName = await _repository.GetAsync(x => x.DepartmentName == model.DepartmentName);
            if (model.DepartmentId == 0)
            {
                if (departmentWithSameName != null)
                    return Response.Failed(DEPT_EXISTS,HttpStatusCode.BadRequest);
                Department department = _mapper.Map<Department>(model);
                department.CreatedBy = UpsertedBy;
                department.UpdatedBy = UpsertedBy;
                await _repository.AddAsync(department);
            }
            else
            {
                Department? existingDepartment = await _repository.GetAsync(x => x.DepartmentId == model.DepartmentId && !x.IsDeleted);
                if (existingDepartment == null)
                {
                    return Response.Failed(DEPT_NOT_FOUND, HttpStatusCode.NotFound);
                }
                if(departmentWithSameName!=null && departmentWithSameName.DepartmentId!=existingDepartment.DepartmentId)
                    return Response.Failed(DEPT_EXISTS,HttpStatusCode.BadRequest);
                Department department = _mapper.Map(model, existingDepartment);
                department.UpdatedAt = DateTime.Now;
                department.UpdatedBy = UpsertedBy;
                _repository.Update(department);
            }
            await _repository.SaveAsync();
            if (model.DepartmentId == 0)
                return Response.Success(DEPT_ADDED,HttpStatusCode.Created);
            return Response.Success(DEPT_UPDATED, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, SAVE_DEPT_CATCH);
            return Response.Failed();
        }
    }

    public async Task<Response> DeleteDepartmentAsync(int id)
    {
        try
        {
            Department? department = await _repository.GetAsync(x => x.DepartmentId == id && !x.IsDeleted, x => x.Users);
            if (department == null)
            {
                return Response.Failed(DEPT_NOT_FOUND, HttpStatusCode.NotFound);
            }
            if (department.Users.Any())
            {
                return Response.Failed(DEPT_WITH_USERS,HttpStatusCode.BadRequest);
            }
            department.IsDeleted = true;
            _repository.Update(department);
            await _repository.SaveAsync();
            return Response.Success(DEPT_DELETED,HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DELETE_DEPT_CATCH, id);
            return Response.Failed();
        }
    }
}
