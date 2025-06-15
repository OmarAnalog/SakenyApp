using Sakenny.Core.Helpers;

namespace SakennyProject.DTO
{
    public class NotificationToReturnDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string? Picture { get; set; }
        public string? Content { get; set; }
        public string To { get; set; }
        public int ? ContentId { get; set; }
        public NotificationType notificationType { get; set; }

    }
}
