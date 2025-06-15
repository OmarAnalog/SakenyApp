using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sakenny.Core.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Rate { get; set; }
        public int CountRated { get; set; } = 0;
        public string? Picture { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public int? ResetPasswordOTP { get; set; }
        public DateTime? ResetPasswordExpiry { get; set; }
        public ICollection<Unit> Units { get; set; } = new HashSet<Unit>();
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<UserNotification> UserNotifications { get; set; }
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
        public ICollection<Connections> Connections { get; set; } = new HashSet<Connections>();
        public ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();
        public ICollection<DeviceToken> DeviceTokens { get; set; }= new HashSet<DeviceToken>();
        public FavouriteList FavouriteList { get; set; } = new FavouriteList();
    }
}
