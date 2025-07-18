using AutoMapper;
using Entity.DTOs;
using Entity.Models;

namespace BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateMap<UpsertUserDTO, User>()
        // .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId == 0 ? (int?)null : src.ManagerId))
        // .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId == 0 ? (int?)null : src.DepartmentId))
        // .ForMember(dest => dest.Manager, opt => opt.Ignore());
        CreateMap<User, UserDTO>()
        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
        .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager.Name ?? ""))
        .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName ?? ""));
        CreateMap<Department, DepartmentDTO>()
        .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));

        // CreateMap<HolidayVM, Holiday>().ReverseMap();
        // CreateMap<UserAttendanceVM, Attendance>().ReverseMap();
        CreateMap<UpsertDepartmentDTO, Department>().ReverseMap();
        CreateMap<RoleDTO, Role>().ReverseMap();
        // CreateMap<LeaveVM, Leave>().ReverseMap();
    }
}
