using System.ComponentModel.DataAnnotations;

namespace SakennyProject.DTO
{
    public class UserProfileEditDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Second name is required")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Address { get; set; }
        public IFormFile? File { get; set; }
        public bool RemovedPicture { get; set; } = false;
    }
}
