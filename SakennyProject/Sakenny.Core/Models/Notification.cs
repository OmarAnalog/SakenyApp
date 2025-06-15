using Sakenny.Core.Helpers;

namespace Sakenny.Core.Models
{
    public class    Notification:BaseEntity
    {
        public string AutherId { get; set; }
        public User Auther { get; set; }
        public string? Content { get; set; }
        public NotificationType type { get; set; }
        public string? To { get; set; } 
        public int ? ContentId { get; set; }
        public DateTime Date { get; set; }= DateTime.UtcNow;
        public ICollection<UserNotification> NotifcationUsers { get; set; } = new HashSet<UserNotification>();
    }
}
