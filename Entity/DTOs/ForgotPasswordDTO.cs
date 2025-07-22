using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs;

public class ForgotPasswordDTO
{
    [MaxLength(50), EmailAddress, Required(ErrorMessage = "Email is Required")]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter a valid email")]
    public string Email { get; set; } = null!;
}
