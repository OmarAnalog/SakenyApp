using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;
using SakennyProject.DTO;
using SakennyProject.Hubs;

namespace Sakenny.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> hub;
        private readonly SakennyDbContext sakennyDb;
        private readonly FCM fcm;

        public NotificationService(IHubContext<NotificationHub> hub,SakennyDbContext sakennyDb,FCM fcm)
        {
            this.hub = hub;
            this.sakennyDb = sakennyDb;
            this.fcm = fcm;
        }
        public async Task<bool> SendMsgNotificatoinAsync(Messages message)
        {

            var user=sakennyDb.Users.Find(message.SenderId);
            if (user == null)
            {
                return false;
            }
            var noti = new NotificationToReturnDto()
            {
                UserId = user.Id,
                ContentId = message.ChatId,
                notificationType = Core.Helpers.NotificationType.Message,
                Name = user.FirstName + " " + user.LastName,
                Picture=user.Picture,
                To=message.ReceiverId,
              };
          
           var res=await SendNotifcation(noti);
            return res; 
        }
        public async Task<bool> SendNotifcation(NotificationToReturnDto notification)
        {
            var rt = new Notification()
                {
                    AutherId = notification.UserId,
                    Content = notification.Content,
                    To = notification.To,
                    type = notification.notificationType,
                    ContentId = notification.ContentId,
                    Date = DateTime.UtcNow,
                };
            sakennyDb.Notifications.Add(rt);
            sakennyDb.SaveChanges();
               var res=await fcm.Send(notification); 
               return res;
            
        }
    }
}
