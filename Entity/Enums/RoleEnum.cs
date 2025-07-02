using System.ComponentModel.DataAnnotations;

namespace Entity.Enums;

public enum RoleEnum
{
    Admin = 1,
    [Display(Name ="Senior Manager")]
    SeniorManager = 2,
    Manager = 3,
    Employee = 4
}
