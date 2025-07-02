using Entity.Enums;

namespace BLL.Helpers;

public static class EnumHelper
{
    public static string RoleToDbString(RoleEnum role)
    {
        return role switch
        {
            RoleEnum.Admin => "Admin",
            RoleEnum.SeniorManager => "Senior Manager",
            RoleEnum.Manager => "Manager",
            RoleEnum.Employee => "Employee",
            _ => "Unknown Role"
        };
    }
    public static string AttendanceToDbString(AttendanceStatusEnum status)
    {
        return status switch
        {
            AttendanceStatusEnum.Present => "Present",
            AttendanceStatusEnum.Absent => "Absent",
            AttendanceStatusEnum.HalfPresent => "Half Present",
            _ => "Unknown Status"
        };
    }
}
