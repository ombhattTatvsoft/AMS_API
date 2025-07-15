using AutoMapper;
using Entity.DTOs;
using Entity.Models;

namespace BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpsertUserDTO, User>()
        .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId == 0 ? (int?)null : src.ManagerId))
        .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId == 0 ? (int?)null : src.DepartmentId))
        .ForMember(dest => dest.Manager, opt => opt.Ignore());
        CreateMap<User, UpsertUserDTO>();
        CreateMap<Department, DepartmentDTO>()
        .ForMember(dest=>dest.UserCount,opt=>opt.MapFrom(src=>src.Users.Count));
        
        // CreateMap<HolidayVM, Holiday>().ReverseMap();
        // CreateMap<UserAttendanceVM, Attendance>().ReverseMap();
        CreateMap<UpsertDepartmentDTO, Department>().ReverseMap();
        // CreateMap<LeaveVM, Leave>().ReverseMap();
    }
}
