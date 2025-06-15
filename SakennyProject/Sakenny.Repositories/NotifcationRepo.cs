using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sakenny.Core.DTO;
using Sakenny.Core.Models;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository.Data;
using SakennyProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Repository
{
    public class NotifcationRepo
    {
        private readonly SakennyDbContext sakennyDb;
        private readonly ILogger<NotifcationRepo> logger;

        public NotifcationRepo(SakennyDbContext sakennyDb,ILogger<NotifcationRepo> logger)
        {
            this.sakennyDb = sakennyDb;
            this.logger = logger;
        }
        public async Task<IReadOnlyList<NotificationDto>> Get(BaseSpecParam param)
        {
            try
            {
                var Noti = await sakennyDb.Notifications.OrderByDescending(n => n.Id)
                .Select(u => new NotificationDto()
                {
                    Id = u.Id,
                    AutherName = $"{(u.Auther != null ? u.Auther.FirstName ?? "" : "")} {(u.Auther != null ? u.Auther.LastName ?? "" : "")}",
                    Created = u.Date,
                    Type = u.type,
                    To = u.To ?? "All User"
                })
                .ToListAsync();

                for (var i = 0; i < Noti?.Count; i++)
                {
                    var notification = Noti[i];
                    if (notification.To != "All User")
                    {
                        var user = await sakennyDb.Users.FindAsync(notification.To);
                        Noti[i].To = $"{user.FirstName} {user.LastName}";
                    }
                }
                return Noti;
            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, ex);
            }
            return new List<NotificationDto>();
        }
       public async Task<IReadOnlyList<NotificationToReturnDto>> GetByUserId(string UserId,BaseSpecParam param)
        {
            var noti = await sakennyDb.Notifications.Where(u => u.To == UserId || u.To=="All User")
                .OrderByDescending(u=>u.Date)
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .Select(u => new NotificationToReturnDto()
                {
                    UserId = u.AutherId,
                    Content = u.Content??"",
                    ContentId = u.Id,
                    Name = u.Auther.FirstName + " " + u.Auther.LastName,
                    notificationType = u.type,
                    Picture = u.Auther.Picture,
                    To = u.To??""

                }).ToListAsync();
            return noti;
        }
        public async Task<SingleNotificationDto> GetById(int Id)

        {
           
            var Noti = await sakennyDb.Notifications.Where(u=>u.Id==Id)
                .Select(u => new SingleNotificationDto()
                {
                    Id = u.Id,
                    AutherName = $"{(u.Auther != null ? u.Auther.FirstName ?? "" : "")} {(u.Auther != null ? u.Auther.LastName ?? "" : "")}",
                    Created = u.Date,
                    Type = u.type,
                    To = u.To ?? "All User",
                    Content = u.Content??""
                }).FirstOrDefaultAsync();
            
            if(Noti==null )return new SingleNotificationDto();
            if (Noti.To != "All User")
            {
                var user = await sakennyDb.Users.FindAsync(Noti.To);
                Noti.To = $"{user.FirstName} {user.LastName}";
            }
            return Noti;
        }
        
    }
}
