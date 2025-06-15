namespace SakennyProject.DTO
{
    public class ReturnedLogin
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string? UserId { get; set; }
    }
}
