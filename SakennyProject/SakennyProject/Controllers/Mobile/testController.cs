using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Models;
using Sakenny.Repository.Data;
using Sakenny.Services.ImageService;
using SakennyProject.DTO;
using SakennyProject.Hubs;
using System.Linq.Expressions;

namespace SakennyProject.Controllers.Mobile
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private readonly ImageService imageService;
        private readonly SakennyDbContext context;
        private readonly FCM fCM;
        private readonly IConfiguration configuration;

        public testController(ImageService imageService,SakennyDbContext context,FCM fCM,IConfiguration configuration)
        {
            this.imageService = imageService;
            this.context = context;
            this.fCM = fCM;
            this.configuration = configuration;
        }
        [HttpPost("upload")]
        public async Task<ActionResult<string>> uploadphoto([FromForm] IFormFile file)
        {
            var res=await imageService.UploadImageAsync(file);
            return Ok(res);
        }
        [HttpGet("get")]
        public async Task<ActionResult<List<Post>>> get()
        {
            var posts = context.Posts.Include(p => p.Unit).ThenInclude(u=>u.PicutresUrl).ToList();
            return Ok(posts);
        }
        [HttpPost]
        public async Task<ActionResult<bool>> Notifcation(NotificationToSendDto Model)
        {
            var userId = "deff1783-59d1-4366-897e-fe8b69920c28";
                 var noti = new Notification()
                 {
                     AutherId = userId,
                     Content = Model.content,
                     Date = Model.DateTime,
                     To = "All User",
                     type = Model.NotificationType,
                 };

            var notification = new NotificationToReturnDto()
            {
                Content = Model.content,
                Name = "abdo",
                To = "3909dffd-1bea-4b7a-a2ee-0f7e451f5334",
                UserId = userId,
                notificationType = Model.NotificationType,
                
            };
            var re=await fCM.Send(notification);
            return Ok(re);
        }
        [HttpGet("Geo")]
        public async Task<ActionResult<List<(string, int)>>> getlist()
        {
            var res =await context.Units.Select(u => new
            {
                u.Location.Longitude,
                u.Location.Latitude,
            }).ToListAsync();
            var list = new List<(double a, double b)>();
            foreach(var item in res)
                list.Add((item.Latitude, item.Latitude));       
            var s = new GeocodingService(configuration["GoogleMapsKey"]);
            var a=await s.GetGovernoratesForCoordinatesList(list);
            var rlist=new List<(string a, int b)>(); 
            foreach(var item in a)
            {
                rlist.Add((item.Key, item.Value));
            }
            return Ok(rlist);
        }

    }
}
