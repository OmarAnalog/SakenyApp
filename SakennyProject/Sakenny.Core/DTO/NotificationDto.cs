using Sakenny.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Core.DTO
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string AutherName { get; set; }
        public string? To { get; set; } = "All User";
        public DateTime Created { get; set; }= DateTime.UtcNow;
        public NotificationType Type { get; set; }


    }
}
