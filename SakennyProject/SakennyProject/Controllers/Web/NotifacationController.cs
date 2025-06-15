using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.DTO;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using Sakenny.Core.Specification;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository;
using Sakenny.Repository.Data;
using Sakenny.Services;
using SakennyProject.DTO;
using SakennyProject.Erorrs;
using SakennyProject.Hubs;
using System.Security.Claims;

namespace SakennyProject.Controllers.Web
{
    [Route("api/web/[controller]")]
    [ApiController]
    //[Authorize(Roles ="Admin")]
    public class NotifacationController : ControllerBase
    {
        private readonly SakennyDbContext sakennyDb;
        private readonly NotifcationRepo repo;
        private readonly IHubContext<NotificationHub> hub;
        private readonly UserManager<User> userManager;
        private readonly NotificationService notificationService;
        private readonly FCM fcm;

        public NotifacationController(SakennyDbContext sakennyDb
            ,NotifcationRepo repo
            ,IHubContext<NotificationHub> hub,
            UserManager<User> userManager,
            NotificationService notificationService,FCM fcm)
        {
            this.sakennyDb = sakennyDb;
            this.repo = repo;
            this.hub = hub;
            this.userManager = userManager;
            this.notificationService = notificationService;
            this.fcm = fcm;
        } 
        [HttpGet("Get")]
        public async Task<ActionResult<NotificationDto>> Get([FromQuery]BaseSpecParam param)
        {
           var res=  await repo.Get(param);
            return Ok(res);
        }
        [HttpPost("SendNoti")]
     
        public async Task<ActionResult<bool>> Send(NotificationToSendDto Model)
        {

             var UserId = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var sender = await userManager.FindByIdAsync(UserId);
            if(sender == null)
                BadRequest(new ApiResponse(400, "where is the user"));
            if (string.IsNullOrEmpty(Model.Email))
            {
               
                var users = await sakennyDb.Users.ToListAsync();
                var noti = new Notification()
                {
                    AutherId = UserId,
                    Content = Model.content,
                    Date = Model.DateTime,
                    To = "All User",
                    type = Model.NotificationType,
                };
                sakennyDb.Notifications.Add(noti);
                await sakennyDb.SaveChangesAsync();
                int NId = noti.Id;
                foreach (var c in users)
                {
                    sakennyDb.UserNotifications.Add(new UserNotification()
                    {
                        NotificationId = NId,
                        UserId = c.Id
                    });

                    var connections = await sakennyDb.Connections.Where(u => u.UserId == UserId).ToListAsync();
                    var notification = new NotificationToReturnDto()
                    {
                        Content = Model.content,
                        notificationType = Model.NotificationType,
                        Name = c.FirstName + " " + c.LastName,
                        Picture = c.Picture,
                        To = c.Id,
                        UserId = UserId,
                    };
                   
                        var res=await fcm.Send(notification); 
                        return Ok(true);
                }
                sakennyDb.SaveChanges();
            }
            else
            {
                var user = await userManager.FindByEmailAsync(Model.Email);
                if (user != null && sender != null)
                {
                    var noti = new Notification()
                    {
                        AutherId = UserId,
                        Content = Model.content,
                        Date = Model.DateTime,
                        To = user.Id,
                        type = Model.NotificationType,

                    };
                    sakennyDb.Notifications.Add(noti);
                    await sakennyDb.SaveChangesAsync();
                    sakennyDb.UserNotifications.Add(new UserNotification()
                    {
                        NotificationId = noti.Id,
                        UserId = UserId
                    });
                    sakennyDb.SaveChanges();
                    
                        var notification = new NotificationToReturnDto()
                        {
                            To = user.Id,
                            Content = Model.content,
                            Name = sender.FirstName + " " + sender.LastName,
                            Picture = sender.Picture,
                            notificationType = Model.NotificationType,
                            UserId = sender.Id,
                        };

                        var res=await fcm.Send(notification);
                        return Ok(res);
                    
                } else
                    return BadRequest(new ApiResponse(400, "where is receiver"));
            }
            return Ok(true);

        }
        [HttpGet("GetById")]
        public async Task<ActionResult<SingleNotificationDto>>GetById(int Id)
        {
            var res=await repo.GetById(Id);
            return Ok(res);
        }
        
    }
}
