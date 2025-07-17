using System.ComponentModel.DataAnnotations;
using Entity.Models;

namespace Entity.DTOs;

public class UserDTO
{
    public int UserId { get; set; }

    [RegularExpression(@"^[A-Za-z]+(?:\s[A-Za-z]+)*$", ErrorMessage = "Enter valid Name")]
    [MaxLength(50), Required(ErrorMessage = "Name is Required")]
    public string Name { get; set; } = null!;

    [MaxLength(50), EmailAddress, Required(ErrorMessage = "Email is Required")]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter valid Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Role is Required")]
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    [Required(ErrorMessage = "Manager is Required")]
    public int ManagerId { get; set; }
    
    public string? ManagerName { get; set; }

    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

}
