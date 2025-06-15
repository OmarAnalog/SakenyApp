namespace SakennyProject.DTO
{
    public class ReturnedDTO
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserId { get; set; }
    }
}
