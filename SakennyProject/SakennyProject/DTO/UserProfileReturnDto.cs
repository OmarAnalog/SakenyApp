namespace SakennyProject.DTO
{
    public class UserProfileReturnDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Address { get; set; }
        public string? Picture { get; set; }
        public int CountRated { get; set; }
        public decimal Rate { get; set; }
    }
}
