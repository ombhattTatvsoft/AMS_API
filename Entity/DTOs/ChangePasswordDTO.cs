using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs;

public class ChangePasswordDTO
{
    public string Email { get; set; } = null!;
    
    [MaxLength(50), Required(ErrorMessage = "Current password is required", AllowEmptyStrings = false)]
    public string CurrentPassword { get; set; } = null!;

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d\s]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character")]
    [MaxLength(50), Required(ErrorMessage = "New password is required", AllowEmptyStrings = false)]
    public string NewPassword { get; set; } = null!;

    [MaxLength(50), Required(ErrorMessage = "Confirm password is required", AllowEmptyStrings = false)]
    [Compare("NewPassword", ErrorMessage = "Password and Confirm Password must match")]
    public string ConfirmNewPassword { get; set; } = null!;
}
