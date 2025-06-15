using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SakennyProject.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
