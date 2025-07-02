using System.ComponentModel.DataAnnotations;

namespace Entity.ViewModels;

public class ForgotPasswordDTO
{
    [MaxLength(50), EmailAddress, Required(ErrorMessage = "Email is Required")]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;
}
