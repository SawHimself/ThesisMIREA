using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class LoginDto
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Display(Name ="Remeber me")]
        public bool RememberMe { get; set; }
    }
}
