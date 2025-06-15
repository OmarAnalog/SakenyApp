using Sakenny.Core.Helpers;

namespace SakennyProject.DTO
{
    public class NotificationToSendDto
    {
        public string? Email { get; set; }
        public string content { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public NotificationType NotificationType { get; set; } 
    }
}
