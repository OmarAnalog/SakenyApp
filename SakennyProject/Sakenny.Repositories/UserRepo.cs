using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sakenny.Core.DTO;
using Sakenny.Core.Helpers;
using Sakenny.Core.Models;
using Sakenny.Core.Specification.SpecParam;
using Sakenny.Repository.Data;

namespace Sakenny.Repository
{
    public class UsersRepo
    {
        private readonly SakennyDbContext sakennyDb;
        private readonly ILogger<UsersRepo> logger;

        public UsersRepo(SakennyDbContext sakennyDb,ILogger<UsersRepo>logger)
        {
            this.sakennyDb = sakennyDb;
            this.logger = logger;
        }
        public async Task<IReadOnlyList<UserToReturnAdminDto>>? Get(UserSpecParam param)
        {
              var AdminID = await sakennyDb.Roles
                .Where(u => u.Name == "Admin").Select(u => u.Id)
                .FirstOrDefaultAsync();
                     var users = await sakennyDb.Users
                    .Where(u => u.IsDeleted == param.Deleted &&
                     (sakennyDb.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == AdminID))==param.Admin)
                    .Skip((param.PageIndex-1)*param.PageIndex) 
                    .Take(param.PageSize)
                    .Select(u => new UserToReturnAdminDto()
                    {
                        Id = u.Id,
                        Name = u.FirstName + " " + u.LastName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber
                    }
                    ).ToListAsync();
                return users;
        }
        public async Task<MyPostsDto> GetMyPosts(BaseSpecParam param,string userId)
        {
            var posts = await sakennyDb.Users.Where(u => u.Id == userId)
                .Include(u => u.Posts)
                .Select(u => u.Posts)
                .FirstOrDefaultAsync();
            var MyPosts=new MyPostsDto();
            MyPosts.Count = posts.Count;
            posts =posts?.Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize).ToList();
            MyPosts.Posts = posts;
            return MyPosts;
        }
        public async Task<List<RatedPostsByUser>>GetRatedPosts(string userId)
        {
            var posts= sakennyDb.Ratings
            .Where(r => r.RatingUserId == userId)
            .Select(r => new RatedPostsByUser
            {
                UnitId = r.UnitId,
                Rate = r.Rate
            }).OrderBy(u=>-u.UnitId);
            return await posts.ToListAsync();
        }
        public async Task<bool> Delete(User user)
        {
            user.IsDeleted = true;

            sakennyDb.Update(user);
            sakennyDb.SaveChanges();
            return true;
        }
        public async Task<bool> Restore(User user)
        {
            user.IsDeleted= false;
            sakennyDb.Update(user);
                sakennyDb.SaveChanges();
            return true;
        }

        public async Task<bool> AddDeviceToken(DeviceTokenSpecParam param)
        {

            try
            {


                var old = await sakennyDb.DeviceTokens.FirstOrDefaultAsync(u => u.Id == param.DeviceToken);
                if (old != null) return true;
                var res = await sakennyDb.DeviceTokens.AddAsync(new DeviceToken()
                {
                    UserId = param.UserId,
                    Id = param.DeviceToken
                });

                sakennyDb.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return true;
        }
        public async Task<bool> RemoveDeviceToken(DeviceTokenSpecParam param)
        {
            var obj = await sakennyDb.DeviceTokens.FindAsync(param.DeviceToken);

            if(obj == null)return true;
            try
            {
                sakennyDb.DeviceTokens.Remove(obj);
                sakennyDb.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return false;
            }
        }
       

    }
}