using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public string? Name { get; set; }
        [StringLength(100)]
        [MaxLength(100)]
        [EmailAddress]
        public string? Address {  get; set; }

        public virtual ICollection<SecretData>? SecretDatas { get; set; }
    }
}
