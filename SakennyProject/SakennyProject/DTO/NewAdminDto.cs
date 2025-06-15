using System.ComponentModel.DataAnnotations;

namespace SakennyProject.DTO
{
    public class NewAdminDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
   
}
