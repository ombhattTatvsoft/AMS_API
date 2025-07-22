using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs;

public class UserLoginDTO
{
    [MaxLength(50), EmailAddress, Required(ErrorMessage = "Email is Required")]
    [RegularExpression(@"^[a-z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter a valid Email")]
    public string Email { get; set; } = null!;

    [MaxLength(50), Required(ErrorMessage = "Password is Required")]
    public string Password { get; set; } = null!;
    
    public bool Rememberme { get; set; }

}
