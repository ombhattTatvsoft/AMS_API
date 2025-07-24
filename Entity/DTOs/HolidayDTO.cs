using System.ComponentModel.DataAnnotations;

namespace Entity.DTOs
{
    public class HolidayDTO
    {
        public int HolidayId { get; set; }

        [MaxLength(100), RegularExpression(@"^[A-Za-z]+(?:\s[A-Za-z]+)*$", ErrorMessage = "Enter valid Name")]
        public string HolidayName { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly HolidayDate { get; set; }
    }
}