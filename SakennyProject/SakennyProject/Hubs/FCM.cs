using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.DTO;
using Sakenny.Core.Helpers;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;
using SakennyProject.DTO;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SakennyProject.Hubs
{
    public class FCM
    {
        private readonly SakennyDbContext sakennyDb;

        public FCM(SakennyDbContext sakennyDb)
        {
            this.sakennyDb = sakennyDb;
        }
        public async Task<bool> Send(NotificationToReturnDto notification)
        {
            var Tokens = new List<DeviceToken>();
            if(notification.To== "All User")
                Tokens=await sakennyDb.DeviceTokens.ToListAsync();
            else
                Tokens=await sakennyDb.DeviceTokens.Where(u=>u.UserId==notification.To).ToListAsync();
             Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "userId", notification.UserId },
                { "notificationType", notification.notificationType.ToString() } ,
                { "Name",notification.Name }
            };
            if (notification.ContentId.HasValue)
            data.Add("contentId", notification.ContentId.ToString());
            else
             data.Add("ContentId", "");
            data.Add("Content",notification.Content??"");
            data.Add("ImageUrl",notification.Picture??"");
            string body = "",title="";
            if (notification.notificationType == Sakenny.Core.Helpers.NotificationType.Message)
            {
                body = $"New Message From {notification.Name}";
                title = "New Message";
            }
            else if (notification.notificationType == Sakenny.Core.Helpers.NotificationType.Comment)
            {
                body = $"New Comment From {notification.Name}";
                title = "New Comment";
            }else if (notification.notificationType == Sakenny.Core.Helpers.NotificationType.Like)
            {
                body = $"Like From{notification.Name} on you post";
                title = "Like";
            }else if (notification.notificationType == Sakenny.Core.Helpers.NotificationType.advertisement)
            {
                body = $"Alert From Sakeny";
                title = "Alert";
            }else
            {
                body = "Advertisement";
                title = "Advertisement";
            }
                var msg = new Message()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = title,
                        Body = body,
                    },
                    Data = data,
                };
            foreach(var token in Tokens)
            {
                msg.Token=token.Id;
                var staste= await FirebaseMessaging.DefaultInstance.SendAsync(msg);
            }
            return true;
        }
    }
}
