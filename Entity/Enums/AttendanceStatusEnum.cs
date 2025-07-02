using System.ComponentModel.DataAnnotations;

namespace Entity.Enums;

public enum AttendanceStatusEnum
{
    Present = 1,
    
    Absent = 2,

    [Display(Name = "Half Present")]
    HalfPresent = 3
}
