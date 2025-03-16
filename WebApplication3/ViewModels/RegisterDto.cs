using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class RegisterDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage ="Password don't match")]
        [Required]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
