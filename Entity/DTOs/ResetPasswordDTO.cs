using System.ComponentModel.DataAnnotations;
namespace Entity.DTOs;

public class ResetPasswordDTO
{
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d\s]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character")]
    [MaxLength(50),Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
    public string NewPassword { get; set; }=null!;

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d\s]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character")]
    [Required(ErrorMessage ="Confirm password required")]
    [MaxLength(50), Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
    public string ConfirmPassword { get; set; }=null!;

    [Required]
    public string ResetCode { get; set; }=null!;
}