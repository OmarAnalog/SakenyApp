using System.ComponentModel.DataAnnotations;

namespace Sakenny.Core.DTO
{
    public class EmailDto
    {
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
